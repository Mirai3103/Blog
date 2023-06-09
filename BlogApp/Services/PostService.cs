using BlogApp.DAL;
using BlogApp.Dto;
using BlogApp.Models;
using BlogApp.Utils;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services;

public class PostService
{
    private readonly BlogContext _context;

    public PostService(BlogContext context)
    {
        _context = context;
    }
    public Post? FindByThumbnailUrl(string thumbnailUrl)
    {
        return _context.Posts.FirstOrDefault(p => p.ThumbnailUrl.Contains(thumbnailUrl));
    }
    public Post? FindById(int id)
    {
        return _context.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
    }
    public ListPostPaginationDto GetPosts(int page = 1, int limit = 16)
    {
        var skip = (page - 1) * limit;
        var posts = _context.Posts.Include(p => p.Tags).Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new Post()
            {
                Author = new User() { DisplayName = p.Author.DisplayName },
                Tags = p.Tags,
                Title = p.Title,
                Slug = p.Slug,
                CreatedAt = p.CreatedAt,
                Id = p.Id,
                Summary = p.Summary,
                ThumbnailUrl = p.ThumbnailUrl
            });
        var totalPage = (int)Math.Ceiling((double)posts.Count() / limit);
        var listPost = posts.Skip(skip).Take(limit).ToList();
        return new ListPostPaginationDto()
        {
            CurrentPage = page,
            TotalPage = totalPage,
            Posts = listPost
        };
    }
    public Post DeletePost(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new Exception("Post not found");
        }
        _context.Posts.Remove(post);
        _context.SaveChanges();
        return post;
    }
    public Post? GetPostBySlug(string slug, bool incrementView = false)
    {
        var post = _context.Posts
            .Where(p => p.Slug == slug)
            .Include(p => p.Tags)
            .Include(p => p.Author)
            .Select(p => new Post()
            {
                Author = new User() { DisplayName = p.Author.DisplayName, Id = p.Author.Id },
                Tags = p.Tags,
                Title = p.Title,
                Slug = p.Slug,
                CreatedAt = p.CreatedAt,
                Id = p.Id,
                Summary = p.Summary,
                ThumbnailUrl = p.ThumbnailUrl,
                Content = p.Content,
            }).FirstOrDefault();
        if (post is not null && incrementView)
        {
            post.ViewCount++;
            _context.SaveChangesAsync();
        }
        return post;
    }

    public Post CreatePost(CreatePostDto createPostDto)
    {
        var post = new Post()
        {
            AuthorId = createPostDto.AuthorId,
            ThumbnailUrl = createPostDto.ThumbnailUrl,
            Content = createPostDto.Content,
            Summary = createPostDto.Summary,
            Title = createPostDto.Title,
            Published = true,
            Slug = Helper.Slugify(createPostDto.Title),
            Tags = _context.Tags.Where(t => createPostDto.Tags.Contains(t.Slug)).ToList()
        };
        _context.Posts.Add(post);
        _context.SaveChanges();

        return post;
    }
    public Post EditPost(EditPostDto editPostDto)
    {
        var post = _context.Posts.Include(p => p.Tags).FirstOrDefault(p => p.Id == editPostDto.Id);
        if (post is null)
        {
            throw new Exception("Post not found");
        }
        post.Content = editPostDto.Content;
        post.Summary = editPostDto.Summary;
        post.Tags = _context.Tags.Where(t => editPostDto.Tags.Contains(t.Slug)).ToList();
        if (post.Title != editPostDto.Title)
        {
            post.Slug = Helper.Slugify(editPostDto.Title);
        }
        post.Title = editPostDto.Title;
        post.AuthorId = editPostDto.AuthorId;
        post.UpdatedAt = DateTime.Now;
        post.ThumbnailUrl = editPostDto.ThumbnailUrl == "" ? post.ThumbnailUrl : editPostDto.ThumbnailUrl;
        _context.SaveChanges();
        return post;
    }
    public void DeletePostBySlug(string slug)
    {
        _context.Posts.Remove(_context.Posts.FirstOrDefault(p => p.Slug == slug));
        _context.SaveChanges();
    }
    public ListPostPaginationDto FindPostsByKeyword(string keyword, int page = 1, int limit = 16)
    {
        var skip = (page - 1) * limit;
        var posts = _context.Posts.Include(p => p.Tags).Include(p => p.Author)
            .Where(p => p.Title.Contains(keyword) || p.Summary.Contains(keyword))
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new Post()
            {
                Author = new User() { DisplayName = p.Author.DisplayName },
                Tags = p.Tags,
                Title = p.Title,
                Slug = p.Slug,
                CreatedAt = p.CreatedAt,
                Id = p.Id,
                Summary = p.Summary,
                ThumbnailUrl = p.ThumbnailUrl
            });
        var totalPage = (int)Math.Ceiling((double)posts.Count() / limit);
        var listPost = posts.Skip(skip).Take(limit).ToList();
        return new ListPostPaginationDto()
        {
            CurrentPage = page,
            TotalPage = totalPage,
            Posts = listPost
        };
    }
}