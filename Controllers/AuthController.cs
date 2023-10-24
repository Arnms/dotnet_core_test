using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using dotnet_server_test.Users;
using dotnet_server_test.Utils;
using dotnet_server_test.Datas;
using dotnet_server_test.Attributes;
using dotnet_server_test.Models.Dtos;
using dotnet_server_test.Constants;

namespace dotnet_server_test.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtUtil _jwtUtil;

    public AuthController(ApplicationDbContext dbContext, IMapper mapper, IJwtUtil jwtUtil)
    {
        _userManager = new UserManager(dbContext, mapper);
        _mapper = mapper;
        _jwtUtil = jwtUtil;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CreateUserDto user)
    {
        var findUser = await _userManager.GetUserByPlatformId(user.PlatformId!);

        if (findUser != null)
        {
            var userToken = findUser.Token != null ? new JwtSecurityTokenHandler().ReadJwtToken(findUser.Token) : _jwtUtil.GenerateJwtToken(findUser);
            var data = userToken.ValidTo > DateTime.Now ? GenerateLoginResponse(findUser, userToken.ValidTo) : await UpdateUserToken(findUser, user);

            return Ok(new DataResponseModel<object>
            {
                Code = (int)ResponseCode.OK,
                Message = "OK",
                Data = data
            });
        }
        else
        {
            return await Register(user);
        }
    }

    [Authorize]
    [HttpGet("validate")]
    public async Task<IActionResult> Validate()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var user = await _userManager.GetUser(v => v.Token == token);

        if (user == null)
        {
            return Unauthorized(new ResponseModel
            {
                Code = (int)ResponseCode.UNAUTHORIZE_NOT_FOUND_USER,
                Message = "Not found user"
            });
        }

        try
        {
            var claimsPrincipal = _jwtUtil.GetTokenClaimsPrincipal(token);

            return Ok(new ResponseModel
            {
                Code = (int)ResponseCode.OK,
                Message = "OK"
            });
        }
        catch (SecurityTokenExpiredException)
        {
            var readToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string platformId = readToken.Subject;

            if (user.PlatformId != platformId)
            {
                return Ok(new DataResponseModel<object>
                {
                    Code = (int)ResponseCode.OK,
                    Message = "OK",
                    Data = UpdateUserToken(user, new CreateUserDto())
                });
            }
            else
            {
                return Unauthorized(new ResponseModel
                {
                    Code = (int)ResponseCode.UNAUTHORIZE_NOT_FOUND_USER,
                    Message = "Not found user"
                });
            }
        }
        catch (Exception)
        {
            return Unauthorized(new ResponseModel
            {
                Code = (int)ResponseCode.UNAUTHORIZE_TOKEN_INVALIDE,
                Message = "Token is invalid"
            });
        }
    }

    private async Task<IActionResult> Register(CreateUserDto user)
    {
        var newUser = _mapper.Map<User>(user);
        var token = _jwtUtil.GenerateJwtToken(newUser);
        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        newUser.Token = accessToken;
        await _userManager.CreateUser(newUser);

        return Ok(new DataResponseModel<object>
        {
            Code = (int)ResponseCode.OK,
            Message = "OK",
            Data = GenerateLoginResponse(newUser, token.ValidTo)
        });
    }

    private async Task<object> UpdateUserToken(User user, CreateUserDto newUser)
    {
        var token = _jwtUtil.GenerateJwtToken(user);
        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        newUser.Token = accessToken;
        await _userManager.UpdateUser(user, _mapper.Map<UpdateUserDto>(newUser));

        return GenerateLoginResponse(user, token.ValidTo);
    }

    private TokenWithUserDto GenerateLoginResponse(User user, DateTime expiration)
    {
        var tokenWithUser = _mapper.Map<TokenWithUserDto>(user);
        tokenWithUser.Expiration = expiration;

        return tokenWithUser;
    }
}