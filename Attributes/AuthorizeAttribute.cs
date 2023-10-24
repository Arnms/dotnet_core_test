using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using dotnet_server_test.Constants;
using dotnet_server_test.Datas;
using dotnet_server_test.Models.Dtos;
using dotnet_server_test.Utils;

namespace dotnet_server_test.Attributes;

public class AuthorizeAttribute : TypeFilterAttribute
{
    public AuthorizeAttribute() : base(typeof(TokenAuthorizationFilter))
    { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtUtil _jwtUtil;

    public TokenAuthorizationFilter(ApplicationDbContext dbContext, IJwtUtil jwtUtil)
    {
        _dbContext = dbContext;
        _jwtUtil = jwtUtil;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        string? token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (token is not null && token is not "")
        {
            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, _jwtUtil.GetTokenValidationParameters(), out var validatedToken);
            var readToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var user = _dbContext.Users.FirstOrDefault(v => v.Token == token && v.PlatformId == readToken.Subject);
        }
        else
        {
            context.Result = new JsonResult(new ResponseModel
            {
                Code = (int)ResponseCode.UNAUTHORIZE_NOT_FOUND_USER,
                Message = "Not found user"
            })
            { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}