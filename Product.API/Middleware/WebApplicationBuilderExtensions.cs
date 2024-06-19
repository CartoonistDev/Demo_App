namespace Product.API.Middleware;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
    {

        var secret = builder.Configuration.GetSection("ApiSettings").GetValue<string>("JwtOptions:Secret");
        var Issuer = builder.Configuration.GetSection("ApiSettings").GetValue<string>("JwtOptions:Issuer");
        var Audience = builder.Configuration.GetSection("ApiSettings").GetValue<string>("JwtOptions:Audience");

        var key = Encoding.ASCII.GetBytes(secret);

        //Configuring authentication for .Net Identity
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience

            };
        });
        return builder;
    }

}