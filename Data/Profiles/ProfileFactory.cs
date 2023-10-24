using dotnet_server_test.Users;

namespace dotnet_server_test.Datas;

public static class ProfileFactory
{
    private static readonly Type[] _profiles = new[] {
        typeof(UserProfile)
    };

    public static Type[] CustomProfiles => _profiles;
}