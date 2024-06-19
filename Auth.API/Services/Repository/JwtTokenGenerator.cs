namespace Auth.API.Services.Repository;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _jwt;
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;

    public JwtTokenGenerator(IOptions<JwtOptions> jwt, IConfiguration config, RoleManager<IdentityRole> roleManager)
    {
        _jwt = jwt.Value;
        _config = config;
        _roleManager = roleManager;
    }
    public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
    {
        int validPeriod = _config.GetSection("ApiSettings").GetValue<int>("JwtOptions:TokenValidity");
        //Generate token based on application user using Jwt Security Token Handler
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwt.Secret);

        var claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName),
            };

        claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = _jwt.Audience,
            Issuer = _jwt.Issuer,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddDays(validPeriod),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
