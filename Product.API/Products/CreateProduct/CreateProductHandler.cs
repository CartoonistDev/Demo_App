namespace Product.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)  
    :   ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);


public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class CreateProductCommandHandler
    (AppDbContext _context, IOptions<Query> query)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var response = new CreateProductResult(Guid.NewGuid());
        var value = query.Value;

        if (value.UseLinq == 1) 
        {


            var product = new OrderProduct
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,

            };
            // Persist to DB
            _context.Add(product);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);

        }
        else
        {
            var id = Guid.NewGuid();

            var sql = $"INSERT INTO OrderProduct (Id, Name, Category, Description, ImageFile, Price) " +
                      $"VALUES (@Id, @Name, @Category, @Description, @ImageFile, @Price)";

            await using var connection = new SqliteConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            await using var insertCommand = new SqliteCommand(sql, connection);
            insertCommand.Parameters.AddWithValue("@Id", id);
            insertCommand.Parameters.AddWithValue("@Name", command.Name);
            insertCommand.Parameters.AddWithValue("@Category", string.Join(",", command.Category));
            insertCommand.Parameters.AddWithValue("@Description", command.Description);
            insertCommand.Parameters.AddWithValue("@ImageFile", command.ImageFile);
            insertCommand.Parameters.AddWithValue("@Price", command.Price);

            await insertCommand.ExecuteNonQueryAsync(cancellationToken);

            return new CreateProductResult(id);
        }

    }
}
