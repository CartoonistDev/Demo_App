
namespace Product.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;


public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}

internal class DeleteProductCommandHandler
    (AppDbContext _context, IOptions<Query> query)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var _query = query.Value;

        if (_query.UseLinq == 1)
        {
            var prod = _context.Products
            .AsNoTracking()
            .Where(x => x.Id == command.Id).FirstOrDefaultAsync();

            var product = prod.Result;

            if (product == null)
            {
                return new DeleteProductResult(false);
            }
            _context.Products.Remove(product);

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
        else
        {
            var deleteSql = "DELETE FROM OrderProduct WHERE Id = @Id";

            await using var connection = new SqliteConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            await using var deleteCommand = new SqliteCommand(deleteSql, connection);
            deleteCommand.Parameters.AddWithValue("@Id", command.Id);

            var rowsAffected = await deleteCommand.ExecuteNonQueryAsync(cancellationToken);

            if (rowsAffected == 0)
            {
                return new DeleteProductResult(false);
            }

            return new DeleteProductResult(true);
        }
        
    }
}
