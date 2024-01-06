using Auth0.ManagementApi.Models;
using Blog.Application.Common.Models;
using Blog.Application.Tags.Queries.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Articles.Queries.Dtos;
public class AuthorDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
public record ArticleDto :BaseAuditableDto
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? ShortDescription { get; init; }
    public string DisplayImage { get; init; } = null!;
    public ICollection<TagDto> Tags { get; init; } = new List<TagDto>();
    public string Content { get; init; } = null!;
    public string? MetaDescription { get; init; }
    public AuthorDto Author { get; set; } = null!;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags));
        }
    }
}
