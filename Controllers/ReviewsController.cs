using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviel.Data;
using Moviel.Models;

namespace Moviel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReviewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Reviews/film/5
    [HttpGet("film/{filmId}")]
    public async Task<ActionResult<IEnumerable<Review>>> GetFilmReviews(int filmId)
    {
        return await _context.Reviews
            .Where(r => r.FilmId == filmId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // GET: api/Reviews/user/5
    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Review>>> GetUserReviews(int userId)
    {
        // Ensure users can only access their own reviews
        var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        if (currentUserId != userId)
        {
            return Forbid();
        }

        return await _context.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.Film)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // POST: api/Reviews
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Review>> CreateReview(Review review)
    {
        // Ensure users can only create reviews for themselves
        var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        if (currentUserId != review.UserId)
        {
            return Forbid();
        }

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFilmReviews), new { filmId = review.FilmId }, review);
    }

    // PUT: api/Reviews/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(int id, Review review)
    {
        if (id != review.Id)
        {
            return BadRequest();
        }

        // Ensure users can only update their own reviews
        var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        if (currentUserId != review.UserId)
        {
            return Forbid();
        }

        _context.Entry(review).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Reviews/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        // Ensure users can only delete their own reviews
        var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        if (currentUserId != review.UserId)
        {
            return Forbid();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewExists(int id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
} 