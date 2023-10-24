using AutoMapper;
using Microsoft.EntityFrameworkCore;
using dotnet_server_test.Datas;

namespace dotnet_server_test.Apps;

public class AppManager
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<App> _app;
    private readonly IMapper _mapper;

    public AppManager(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _app = _dbContext.Apps;
        _mapper = mapper;
    }

    public async Task<App?> GetApp() => await _app.FirstOrDefaultAsync();
}