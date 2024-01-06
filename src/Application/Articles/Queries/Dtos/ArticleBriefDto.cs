using Blog.Application.Common.Models;
using Blog.Application.Tags.Queries.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Articles.Queries.Dtos;

public record ArticleBriefDto():BaseAuditableDto
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? ShortDescription { get; init; }
    public string DisplayImage { get; init; } = null!;
    public ICollection<TagDto> Tags { get; init; } = new List<TagDto>();
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Article, ArticleBriefDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags));
        }
    }
}
