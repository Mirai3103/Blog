using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Mappings;
using Blog.Application.Common.Models;
using Blog.Application.Tags.Queries.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Tags.Queries.GetAllTags;

public record GetAllTagsQuery : PaginationQuery,IRequest<PaginatedList<TagDto>>
{
    public GetAllTagsQuery():base()
    {
    }
}

public class GetAllTagsQueryValidator : AbstractPaginationQueryValidator<GetAllTagsQuery>
{
    public GetAllTagsQueryValidator():base()
    {
        
    }
}

public class GetAllTagsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAllTagsQuery, PaginatedList<TagDto>>
{
    private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .OrderByDescending(x => x.Id)
            .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
