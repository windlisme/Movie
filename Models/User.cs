using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("role")]
    public string Role { get; set; } = "user";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<WatchHistory> WatchHistory { get; set; } = new List<WatchHistory>();
} 