using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DAL;

public class BlogContext:DbContext
{
    public DbSet<Comment> Comments { get; set; } =null!;
    public DbSet<Post> Posts { get; set; } =null!;
    public DbSet<Tag> Tags { get; set; } =null!;
    public DbSet<User> Users { get; set; } =null!;

    protected BlogContext(): base()
    {
    }

    public BlogContext(DbContextOptions options) : base(options)
    {
    }
}