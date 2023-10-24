using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using dotnet_server_test.Users;

namespace dotnet_server_test.Utils;

public interface IJwtUtil
{
    public JwtSecurityToken GenerateJwtToken(User user);
    public TokenValidationParameters GetTokenValidationParameters();
    public ClaimsPrincipal GetTokenClaimsPrincipal(string token);
}

public class JwtUtil : IJwtUtil
{
    private readonly IConfiguration _config;

    public JwtUtil(IConfiguration config)
    {
        _config = config;
    }

    public JwtSecurityToken GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.PlatformId!),
                new Claim("Provider", Enum.GetName(user.Provider)!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddDays(14),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public TokenValidationParameters GetTokenValidationParameters()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];

        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ClockSkew = TimeSpan.Zero
        };
    }

    public ClaimsPrincipal GetTokenClaimsPrincipal(string token)
    {
        return new JwtSecurityTokenHandler().ValidateToken(token, GetTokenValidationParameters(), out var validatedToken);
    }
}