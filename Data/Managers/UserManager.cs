using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using dotnet_server_test.Datas;

namespace dotnet_server_test.Users;

public class UserManager
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<User> _user;
    private readonly IMapper _mapper;

    public UserManager(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _user = _dbContext.Users;
        _mapper = mapper;
    }

    public async Task<int> CreateUser(User user)
    {
        _user.Add(user);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateUser(User user, UpdateUserDto updateUser)
    {
        _mapper.Map(updateUser, user);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUser(Expression<Func<User, bool>> expression) => await _user.FirstOrDefaultAsync(expression);

    public async Task<User?> GetUserByPlatformId(string platformId) => await _user.FirstOrDefaultAsync(user => user.PlatformId == platformId);
}