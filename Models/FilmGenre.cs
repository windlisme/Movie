using System.ComponentModel.DataAnnotations.Schema;

namespace Moviel.Models;

public class FilmGenre
{
    [Column("film_id")]
    public int FilmId { get; set; }
    public Film Film { get; set; } = null!;

    [Column("genre_id")]
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
} 