using Blog.Domain.Entities;

namespace Blog.Application.Tags.Queries.Dtos;

public class TagDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}
