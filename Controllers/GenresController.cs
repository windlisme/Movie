using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviel.Attributes;
using Moviel.Data;
using Moviel.Models;

namespace Moviel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GenresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Genres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        return await _context.Genres.ToListAsync();
    }

    // GET: api/Genres/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        var genre = await _context.Genres.FindAsync(id);

        if (genre == null)
        {
            return NotFound();
        }

        return genre;
    }

    // POST: api/Genres
    [HttpPost]
    // [Authorize]
    public async Task<ActionResult<Genre>> CreateGenre(Genre genre)
    {
        // // Only admin can create genres
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
    }

    // PUT: api/Genres/5
    [HttpPut("{id}")]
    // [Authorize]
    public async Task<IActionResult> UpdateGenre(int id, Genre genre)
    {
        // // Only admin can update genres
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        if (id != genre.Id)
        {
            return BadRequest();
        }

        _context.Entry(genre).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GenreExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Genres/5
    [HttpDelete("{id}")]
    // [Authorize]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        // // Only admin can delete genres
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GenreExists(int id)
    {
        return _context.Genres.Any(e => e.Id == id);
    }
} 