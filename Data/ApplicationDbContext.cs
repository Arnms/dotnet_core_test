using Microsoft.EntityFrameworkCore;
using dotnet_server_test.Apps;
using dotnet_server_test.Leaderboards;
using dotnet_server_test.Users;

namespace dotnet_server_test.Datas;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<App> Apps => Set<App>();
    public DbSet<Leaderboard> Leaderboards => Set<Leaderboard>();
    public DbSet<RankHistory> RankHistories => Set<RankHistory>();
    public DbSet<ScoreHistory> ScoreHistories => Set<ScoreHistory>();
    public DbSet<User> Users => Set<User>();
}