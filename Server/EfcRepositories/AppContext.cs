using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>()
        .HasOne(p => p.User)
        .WithMany(u => u.Posts)
        .HasForeignKey(p => p.UserId);

    modelBuilder.Entity<Comment>()
        .HasOne(c => c.User)
        .WithMany(u => u.Comments)
        .HasForeignKey(c => c.UserId);

    modelBuilder.Entity<Comment>()
        .HasOne(c => c.Post)
        .WithMany(p => p.Comments)
        .HasForeignKey(c => c.PostId);

    modelBuilder.Entity<User>().HasData(
        new { Id = 1, UserName = "admin", Password = "admin123" },
        new { Id = 2, UserName = "john", Password = "pass123" },
        new { Id = 3, UserName = "jane", Password = "pass456" }
    );

    modelBuilder.Entity<Post>().HasData(
        new { Id = 1, Title = "Welcome", Body = "Welcome to the forum!", UserId = 1 },
        new { Id = 2, Title = "First Post", Body = "This is my first post", UserId = 2 },
        new { Id = 3, Title = "Question", Body = "How does this work?", UserId = 3 }
    );

    modelBuilder.Entity<Comment>().HasData(
        new { Id = 1, Body = "Great post!", UserId = 2, PostId = 1 },
        new { Id = 2, Body = "Thanks for sharing", UserId = 3, PostId = 1 },
        new { Id = 3, Body = "I agree", UserId = 1, PostId = 2 }
    );
}
}