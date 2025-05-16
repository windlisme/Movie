using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class Review
{
    [Key]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Column("film_id")]
    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;

    [Required]
    [Column("rating")]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Column("review_text")]
    public string? ReviewText { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 