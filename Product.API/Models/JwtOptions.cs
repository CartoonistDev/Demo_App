namespace Product.API.Models;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string baseUrl { get; set; }
}

public class Query
{
    public int UseLinq { get; set; }
}