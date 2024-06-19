namespace Product.API.Products.GetProductById;

public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(OrderProduct Product);

internal class GetProductByIdQueryHandler
    (AppDbContext _context, IOptions<Query> _query)
     : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>

{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var _query1 = _query.Value;

        if (_query1.UseLinq == 1)
        {
            var product = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.id);

            if (product == null)
            {
                throw new NotFoundException($"Product with Id:::{query.id} Not found");
            }

            return new GetProductByIdResult(product);
        }
        else 
        {
            // Define the SQL query to select the product by ID
            var selectSql = "SELECT * FROM OrderProduct WHERE Id = @Id";

            // Create the connection and command objects
            await using var connection = new SqliteConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            // Create and execute the select command
            await using var selectCommand = new SqliteCommand(selectSql, connection);
            selectCommand.Parameters.AddWithValue("@Id", query.id);

            await using var reader = await selectCommand.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                throw new NotFoundException($"Product with Id:::{query.id} Not found");
            }

            var product = new OrderProduct
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Category = reader.GetString(reader.GetOrdinal("Category")).Split(',').ToList(),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                ImageFile = reader.GetString(reader.GetOrdinal("ImageFile")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };

            return new GetProductByIdResult(product);
        }
            
    }
}
