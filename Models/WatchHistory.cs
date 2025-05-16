using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class WatchHistory
{
    [Key]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Column("film_id")]
    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;

    [Column("watched_at")]
    public DateTime WatchedAt { get; set; } = DateTime.UtcNow;
} 