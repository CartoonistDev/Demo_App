namespace Product.API.Middleware;
public interface IUtility
{
    string GetLoggedInToken();
}
public class Utility : IUtility
{ 
    private static IHttpContextAccessor httpContext;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor;
    }


    public string GetLoggedInToken()
    {
        var authHeader = httpContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault(m => m != null && m.StartsWith("Bearer"));

        if (authHeader != null) 
        {
            return authHeader;
        }

        return string.Empty;
    }

}
