using BlogApp.DAL;
using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services;

public class TagService
{
    private readonly BlogContext _context;
    public TagService(BlogContext context)
    {
        _context = context;
    }
    public ICollection<Tag> GetTags()
    {
        return _context.Tags.ToList();
    }
    public Tag GetTag(string slug)
    {
        return _context.Tags.FirstOrDefault(t => t.Slug == slug);
    }
    public bool TagExists(string slug)
    {
        return _context.Tags.Any(t => t.Slug == slug);
    }
    public async Task<bool> CreateTag(Tag tag)
    {
        tag.Slug = BlogApp.Utils.Helper.Slugify(tag.Name);
        var isExisted = TagExists(tag.Slug);
        if (isExisted)
        {
            return false;
        }
        _context.Add(tag);
        await _context.SaveChangesAsync();
        return true;
    }
    public IEnumerable<Post> GetPostsByTag(string slug)
    {
        var tag = GetTag(slug);
        var posts = _context.Posts
            .Include(p => p.Tags)
            .Include(p => p.Author)
            .Where(p => p.Tags.Contains(tag))
            .Select(p => new Post
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                Summary = p.Summary,
                CreatedAt = p.CreatedAt,
                ThumbnailUrl = p.ThumbnailUrl,
                ViewCount = p.ViewCount,
                AuthorId = p.AuthorId,
                Author = p.Author,
                Tags = p.Tags,
                Comments = p.Comments
            });
        return posts;
    }
}