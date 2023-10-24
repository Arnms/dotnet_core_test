using static dotnet_server_test.Constants.Constants;

namespace dotnet_server_test.Users;

public class TokenWithUserDto
{
    public required PlatformProvider Provider { get; set; }
    public required string PlatformId { get; set; }
    public required float AppVersion { get; set; }
    public required string Nickname { get; set; }
    public required string Token { get; set; }

    public required DateTime Expiration { get; set; }
}