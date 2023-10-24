namespace dotnet_server_test.Users;

public class UserProfile : AutoMapper.Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<CreateUserDto, UpdateUserDto>();
        CreateMap<User, TokenWithUserDto>();
    }
}