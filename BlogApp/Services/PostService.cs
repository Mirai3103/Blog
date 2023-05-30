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

    public Post? GetPostBySlug(string slug)
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
        };
        _context.Posts.Add(post);
        _context.SaveChanges();

        return post;
    }
    public Post EditPost(EditPostDto editPostDto)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == editPostDto.Id);
        if (post is null)
        {
            throw new Exception("Post not found");
        }
        post.Content = editPostDto.Content;
        post.Summary = editPostDto.Summary;
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
}