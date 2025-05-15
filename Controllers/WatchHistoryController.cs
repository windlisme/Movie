using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviel.Data;
using Moviel.Models;

namespace Moviel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchHistoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WatchHistoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/WatchHistory/user/5
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Film>>> GetUserWatchHistory(int userId)
    {
        return await _context.WatchHistory
            .Where(w => w.UserId == userId)
            .Include(w => w.Film)
                .ThenInclude(f => f.FilmGenres)
                    .ThenInclude(fg => fg.Genre)
            .OrderByDescending(w => w.WatchedAt)
            .Select(w => w.Film)
            .ToListAsync();
    }

    // POST: api/WatchHistory
    [HttpPost]
    public async Task<ActionResult<WatchHistory>> AddToWatchHistory(WatchHistory watchHistory)
    {
        _context.WatchHistory.Add(watchHistory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserWatchHistory), new { userId = watchHistory.UserId }, watchHistory);
    }

    // DELETE: api/WatchHistory/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWatchHistory(int id)
    {
        var watchHistory = await _context.WatchHistory.FindAsync(id);
        if (watchHistory == null)
        {
            return NotFound();
        }

        _context.WatchHistory.Remove(watchHistory);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 