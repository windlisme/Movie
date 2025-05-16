using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviel.Data;
using Moviel.Models;

namespace Moviel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FavoritesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Favorites/user/5
    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Film>>> GetUserFavorites(int userId)
    {
        // // Ensure users can only access their own favorites
        // var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        // if (currentUserId != userId)
        // {
        //     return Forbid();
        // }

        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Film)
                .ThenInclude(f => f.FilmGenres)
                    .ThenInclude(fg => fg.Genre)
            .Select(f => f.Film)
            .ToListAsync();
    }

    // POST: api/Favorites
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Favorite>> AddFavorite(Favorite favorite)
    {
        // Ensure users can only add favorites for themselves
        var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        if (currentUserId != favorite.UserId)
        {
            return Forbid();
        }

        _context.Favorites.Add(favorite);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (FavoriteExists(favorite.UserId, favorite.FilmId))
            {
                return Conflict();
            }
            throw;
        }

        return CreatedAtAction(nameof(GetUserFavorites), new { userId = favorite.UserId }, favorite);
    }

    // DELETE: api/Favorites/5/5
    [HttpDelete("{userId}/{filmId}")]
    [Authorize]
    public async Task<IActionResult> RemoveFavorite(int userId, int filmId)
    {
        // Ensure users can only remove their own favorites
        var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        if (currentUserId != userId)
        {
            return Forbid();
        }

        var favorite = await _context.Favorites.FindAsync(userId, filmId);
        if (favorite == null)
        {
            return NotFound();
        }

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FavoriteExists(int userId, int filmId)
    {
        return _context.Favorites.Any(e => e.UserId == userId && e.FilmId == filmId);
    }
} 