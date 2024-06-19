namespace Product.API.Products.GetProductAndCustomerDetails;

public record GetProductAndCustomerDetailsQuery(string CustomerEmail) : IQuery<GetProductAndCustomerDetailsResult>;
public record GetProductAndCustomerDetailsResult(UserProducts Product);

internal class GetProductAndCustomerDetailsQueryHandler
    (AppDbContext _context, IRestClientService restClient, IUtility utility, IOptions<JwtOptions> options)
     : IQueryHandler<GetProductAndCustomerDetailsQuery, GetProductAndCustomerDetailsResult>
{
    public async Task<GetProductAndCustomerDetailsResult> Handle(GetProductAndCustomerDetailsQuery query, CancellationToken cancellationToken)
    {
        UserProducts products = new UserProducts();
        var jwtOptions = options.Value;
        string token = utility.GetLoggedInToken();

        if (!string.IsNullOrEmpty(token))
        {
            string[] tokenParts = token.Split(' ');

            if (tokenParts.Length == 2 && tokenParts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "Authorization", $"{tokenParts[0]} {tokenParts[1]}" }
                };

                string url = jwtOptions.baseUrl + "api/user/getUserByEmail";


                var user = await restClient.PostAsync<UserDetailsResponse, string>(url, query.CustomerEmail, headers);

                if (user != null)
                {
                    var product = await _context.Products
                        .AsNoTracking().FirstOrDefaultAsync();

                    if (product == null)
                    {
                        throw new NotFoundException($"Product with Id:::{query.CustomerEmail} Not found");
                    }
                    products = new UserProducts()
                    {
                        UserEmail = user.data.EmailAddress,
                        PhoneNumber = user.data.PhoneNumber,
                        ProductDescription = product.Description,
                        ProductId = product.Id.ToString(),
                        ProductName = product.Name,
                    };
                }

            }
            else
            {
                return new GetProductAndCustomerDetailsResult(products);

            }

            return new GetProductAndCustomerDetailsResult(products);

        }
        else
        {
            return new GetProductAndCustomerDetailsResult(products);

        }
    }
}
