using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviel.Attributes;
using Moviel.Data;
using Moviel.Models;

namespace Moviel.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Keep basic authentication
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        // // Check if current user is admin
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        return await _context.Users
            .Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        // // Check if current user is admin or requesting their own data
        // var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        // var currentUserRole = User.FindFirst("role")?.Value;
        
        // if (currentUserRole != "admin" && currentUserId != id)
        // {
        //     return Forbid();
        // }

        var user = await _context.Users
            .Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Role = u.Role,
                PasswordHash = u.PasswordHash,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        // // Only admin can create new users
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        // // Check if current user is admin or updating their own data
        // var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
        // var currentUserRole = User.FindFirst("role")?.Value;
        
        // if (currentUserRole != "admin" && currentUserId != id)
        // {
        //     return Forbid();
        // }

        if (id != user.Id)
        {
            return BadRequest();
        }

        // // If not admin, prevent role changes
        // if (currentUserRole != "admin")
        // {
        //     var existingUser = await _context.Users.FindAsync(id);
        //     if (existingUser != null)
        //     {
        //         user.Role = existingUser.Role; // Keep existing role
        //     }
        // }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        // // Only admin can delete users
        // var currentUserRole = User.FindFirst("role")?.Value;
        // if (currentUserRole != "admin")
        // {
        //     return Forbid();
        // }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
} 