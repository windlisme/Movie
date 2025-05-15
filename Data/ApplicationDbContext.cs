using Microsoft.EntityFrameworkCore;
using Moviel.Models;

namespace Moviel.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Disable migrations
        Database.SetCommandTimeout(30);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<FilmGenre> FilmGenres { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<WatchHistory> WatchHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Film>().ToTable("films");
        modelBuilder.Entity<Genre>().ToTable("genres");
        modelBuilder.Entity<FilmGenre>().ToTable("film_genres");
        modelBuilder.Entity<Favorite>().ToTable("favorites");
        modelBuilder.Entity<Review>().ToTable("reviews");
        modelBuilder.Entity<WatchHistory>().ToTable("watch_history");

        // Configure composite keys
        modelBuilder.Entity<FilmGenre>()
            .HasKey(fg => new { fg.FilmId, fg.GenreId });

        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.FilmId });

        // Configure relationships
        modelBuilder.Entity<FilmGenre>()
            .HasOne(fg => fg.Film)
            .WithMany(f => f.FilmGenres)
            .HasForeignKey(fg => fg.FilmId);

        modelBuilder.Entity<FilmGenre>()
            .HasOne(fg => fg.Genre)
            .WithMany(g => g.FilmGenres)
            .HasForeignKey(fg => fg.GenreId);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Film)
            .WithMany(f => f.Favorites)
            .HasForeignKey(f => f.FilmId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Film)
            .WithMany(f => f.Reviews)
            .HasForeignKey(r => r.FilmId);

        modelBuilder.Entity<WatchHistory>()
            .HasOne(w => w.User)
            .WithMany(u => u.WatchHistory)
            .HasForeignKey(w => w.UserId);

        modelBuilder.Entity<WatchHistory>()
            .HasOne(w => w.Film)
            .WithMany(f => f.WatchHistory)
            .HasForeignKey(w => w.FilmId);
    }
} 