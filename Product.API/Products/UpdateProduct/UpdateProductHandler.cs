namespace Product.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 to 150 characters");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler
    (AppDbContext _context, IOptions<Query> _query)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>

{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var _query1 = _query.Value;

        if (_query1.UseLinq == 1)
        {
            var product = _context.Products.AsNoTracking()
                .Where(x => x.Id == command.Id).FirstOrDefaultAsync();
            var prod = product.Result;
            if (prod is null)
            {
                throw new NotFoundException("");
            }

            prod.Name = command.Name;
            prod.Category = command.Category;
            prod.Description = command.Description;
            prod.Price = command.Price;
            prod.ImageFile = command.ImageFile;

            _context.Update(product);

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
        else
        {
            var updateSql = @"UPDATE OrderProduct
                            SET Name = @Name, Category = @Category, Description = @Description, Price = @Price, ImageFile = @ImageFile
                            WHERE Id = @Id";

            await using var connection = new SqliteConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);


            await using var updateCommand = new SqliteCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@Id", command.Id);
            updateCommand.Parameters.AddWithValue("@Name", command.Name);
            updateCommand.Parameters.AddWithValue("@Category", string.Join(",", command.Category));
            updateCommand.Parameters.AddWithValue("@Description", command.Description);
            updateCommand.Parameters.AddWithValue("@Price", command.Price);
            updateCommand.Parameters.AddWithValue("@ImageFile", command.ImageFile);

            await updateCommand.ExecuteNonQueryAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
            
    }
}
