using Blog.Application.Common.Interfaces;
using Blog.Application.Tags.Queries.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Tags.Queries.GetTagBySlug;

public record GetTagBySlugQuery(string Slug) : IRequest<TagDto>
{
}

public class GetTagBySlugQueryValidator : AbstractValidator<GetTagBySlugQuery>
{
    public GetTagBySlugQueryValidator()
    {
        RuleFor(v => v.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");
    }
}

public class GetTagBySlugQueryHandler : IRequestHandler<GetTagBySlugQuery, TagDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TagDto> Handle(GetTagBySlugQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .Where(t => t.Slug == request.Slug)
            .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return tag == null ? throw new NotFoundException(nameof(Tag), request.Slug) : tag;
    }
}
