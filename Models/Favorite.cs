using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class Favorite
{
    [Column("user_id")]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Column("film_id")]
    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 