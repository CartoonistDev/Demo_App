namespace Product.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductResult>;
public record GetProductResult(IEnumerable<OrderProduct> Products);
internal class GetProductQueryHandler(AppDbContext _context, IOptions<Query> _query)
   : IQueryHandler<GetProductsQuery, GetProductResult>
{
    public async Task<GetProductResult> Handle([AsParameters]GetProductsQuery query, CancellationToken cancellationToken)
    {
        var _query1 = _query.Value;

        if (_query1.UseLinq == 1)
        {
            var products = await _context.Products.AsNoTracking()
            .OrderBy(p => p.Category)
            .Skip((query.PageNumber ?? 1 - 1) * (query.PageSize ?? 10))
            .Take(query.PageSize ?? 10)
            .ToListAsync(cancellationToken);

            if (products.Count < 1)
            {
                return new GetProductResult(null);
            }
            return new GetProductResult(products);
        }
        else
        {
            int pageNumber = query.PageNumber ?? 1;
            int pageSize = query.PageSize ?? 10;
            int offset = (pageNumber - 1) * pageSize;

            var selectSql = "SELECT * FROM OrderProduct ORDER BY Category LIMIT @Limit OFFSET @Offset";

            await using var connection = new SqliteConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            await using var selectCommand = new SqliteCommand(selectSql, connection);
            selectCommand.Parameters.AddWithValue("@Limit", pageSize);
            selectCommand.Parameters.AddWithValue("@Offset", offset);

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
                return new GetProductResult(null);
            }

            return new GetProductResult(products);
        }
    }
}