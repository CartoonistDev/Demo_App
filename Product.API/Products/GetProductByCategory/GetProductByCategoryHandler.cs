using Product.API.Data;

namespace Product.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<OrderProduct> Products);

public class GetProductByCategoryQueryHandler(AppDbContext _context, IOptions<Query> _query)
        : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>

{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var _query1 = _query.Value;

        if (_query1.UseLinq == 1)
        {
            var products = await _context.Products.AsNoTracking()
               .Where(p => p.Category.Contains(query.Category))
               .ToListAsync(cancellationToken);

            if (products.Count < 1)
            {
                throw new NotFoundException($"Product with Category::: {query.Category} Not Found");
            }

            return new GetProductByCategoryResult(products);
        }
        else
        {
            var selectSql = "SELECT * FROM OrderProduct WHERE Category LIKE '%' || @Category || '%'";

            await using var connection = new SqliteConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            await using var selectCommand = new SqliteCommand(selectSql, connection);
            selectCommand.Parameters.AddWithValue("@Category", query.Category);

            var products = new List<OrderProduct>();

            await using var reader = await selectCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var product = new OrderProduct
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Category = reader.GetString(reader.GetOrdinal("Category")).Split(',').ToList(),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    ImageFile = reader.GetString(reader.GetOrdinal("ImageFile")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                };
                products.Add(product);
            }

            if (products.Count < 1)
            {
                throw new NotFoundException($"Product with Category::: {query.Category} Not Found");
            }

            return new GetProductByCategoryResult(products);
        }
           
    }
}
