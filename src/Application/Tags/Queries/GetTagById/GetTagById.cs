using Blog.Application.Common.Interfaces;
using Blog.Application.Tags.Queries.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Tags.Queries.GetTagById;

public record GetTagByIdQuery(int TagId) : IRequest<TagDto>
{
}

public class GetTagByIdQueryValidator : AbstractValidator<GetTagByIdQuery>
{
    public GetTagByIdQueryValidator()
    {
        RuleFor(v => v.TagId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
    }
}

public class GetTagByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTagByIdQuery, TagDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags.FindAsync(request.TagId, cancellationToken);
        if (tags == null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId.ToString());
        }
        return new TagDto
        {
            Id = tags.Id,
            Name = tags.Name,
            Slug = tags.Slug
        };
    }
}
