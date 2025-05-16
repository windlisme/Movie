using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    // Navigation properties
    public ICollection<FilmGenre> FilmGenres { get; set; } = new List<FilmGenre>();
} 