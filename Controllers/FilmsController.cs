using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviel.Attributes;
using Moviel.Data;
using Moviel.Models;

namespace Moviel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FilmsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Films/all
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Film>>> GetAllFilms()
    {
        return await _context.Films.ToListAsync();
    }

    // GET: api/Films
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Film>>> GetFilms(
        [FromQuery] string? search,
        [FromQuery] int? year,
        [FromQuery] int? genreId,
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 20)
    {
        var query = _context.Films
            .Include(f => f.FilmGenres)
                .ThenInclude(fg => fg.Genre)
            .AsQueryable();

        // Search by title only
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(f => f.Title.ToLower().Contains(search));
        }

        // Filter by year
        if (year.HasValue)
        {
            query = query.Where(f => f.ReleaseYear == year);
        }

        // Filter by genre
        if (genreId.HasValue)
        {
            query = query.Where(f => f.FilmGenres.Any(fg => fg.GenreId == genreId));
        }

        // Calculate total count for pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var skip = (page.Value - 1) * pageSize.Value;
        query = query.Skip(skip).Take(pageSize.Value);

        // Get the results
        var films = await query.ToListAsync();

        // Add pagination metadata to response headers
        Response.Headers.Add("X-Total-Count", totalCount.ToString());
        Response.Headers.Add("X-Total-Pages", Math.Ceiling((double)totalCount / pageSize.Value).ToString());
        Response.Headers.Add("X-Current-Page", page.Value.ToString());

        return films;
    }

    // GET: api/Films/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Film>> GetFilm(int id)
    {
        var film = await _context.Films
            .Include(f => f.FilmGenres)
                .ThenInclude(fg => fg.Genre)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (film == null)
        {
            return NotFound();
        }

        return film;
    }

    // PUT: api/Films/5/video-url
    [HttpPut("{id}/video-url")]
    public async Task<ActionResult<Film>> UpdateVideoUrl(int id, [FromBody] VideoUrlUpdateRequest request)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            return NotFound();
        }

        film.VideoUrl = request.VideoUrl;
        await _context.SaveChangesAsync();

        return await GetFilm(id);
    }

    // POST: api/Films
    [HttpPost]
    // [Authorize]
    public async Task<ActionResult<Film>> CreateFilm(Film film)
    {
        // // Only admin can create films
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        _context.Films.Add(film);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
    }

    // PUT: api/Films/5
    [HttpPut("{id}")]
    // [Authorize]
    public async Task<IActionResult> UpdateFilm(int id, Film film)
    {
        // // Only admin can update films
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        if (id != film.Id)
        {
            return BadRequest();
        }

        _context.Entry(film).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FilmExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Films/5
    [HttpDelete("{id}")]
    // [Authorize]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        // // Only admin can delete films
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            return NotFound();
        }

        _context.Films.Remove(film);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FilmExists(int id)
    {
        return _context.Films.Any(e => e.Id == id);
    }
}

public class VideoUrlUpdateRequest
{
    public string VideoUrl { get; set; } = string.Empty;
} 