using static dotnet_server_test.Constants.Constants;

namespace dotnet_server_test.Users;

public class UpdateUserDto
{
    public float? AppVersion { get; set; }
    public UserType? UserType { get; set; }
    public string? Nickname { get; set; }
    public string? Token { get; set; }
}