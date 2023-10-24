using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnet_server_test.Apps;

[Table("app")]
[PrimaryKey(nameof(Id))]
public class App
{
    [Column("id")]
    public int Id { get; set; }

    [Column("version")]
    public float Version { get; set; }

    [Column("season")]
    public int Season { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}