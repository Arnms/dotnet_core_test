using static dotnet_server_test.Constants.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using dotnet_server_test.Leaderboards;

namespace dotnet_server_test.Users;

[Table("user")]
[PrimaryKey(nameof(Id))]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("provider")]
    public PlatformProvider Provider { get; set; }

    [Column("platform_id")]
    public string? PlatformId { get; set; }

    [Column("app_version")]
    public float AppVersion { get; set; }

    [Column("user_type")]
    public UserType UserType { get; set; }

    [Column("user_status")]
    public UserStatus UserStatus { get; set; }

    [Column("nickname")]
    public string? Nickname { get; set; }

    [Column("token")]
    public string? Token { get; set; }

    [Column("created_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public List<Leaderboard> Leaderboards { get; } = new();
    public List<RankHistory> RankHistories { get; } = new();
    public List<ScoreHistory> ScoreHistories { get; } = new();
}