using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class Film
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = null!;
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("release_year")]
    public int? ReleaseYear { get; set; }
    
    [Column("duration")]
    public int? Duration { get; set; }
    
    [Column("rating")]
    public string? Rating { get; set; }
    
    [Column("cover_url")]
    public string? CoverUrl { get; set; }
    
    [Column("video_url")]
    public string? VideoUrl { get; set; }
    
    [Column("torrent_url")]
    public string? TorrentUrl { get; set; }
    
    [Required]
    [Column("status")]
    public string Status { get; set; } = "pending";
    
    [Column("last_checked_at")]
    public DateTime? LastCheckedAt { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<FilmGenre> FilmGenres { get; set; } = new List<FilmGenre>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<WatchHistory> WatchHistory { get; set; } = new List<WatchHistory>();
} 