using static dotnet_server_test.Constants.Constants;

namespace dotnet_server_test.Users;

public class CreateUserDto : UpdateUserDto
{
    public PlatformProvider? Provider { get; set; }
    public string? PlatformId { get; set; }
}