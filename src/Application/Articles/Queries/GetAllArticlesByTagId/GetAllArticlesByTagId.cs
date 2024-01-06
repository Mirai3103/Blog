using Blog.Application.Articles.Queries.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Mappings;
using Blog.Application.Common.Models;

namespace Blog.Application.Articles.Queries.GetAllArticlesByTagId;

public record GetAllArticlesByTagIdQuery(int TagId) :PaginationQuery, IRequest<PaginatedList<ArticleBriefDto>>
{
}

public class GetAllArticlesByTagIdQueryValidator : AbstractValidator<GetAllArticlesByTagIdQuery>
{
    public GetAllArticlesByTagIdQueryValidator()
    {
    }
}

public class GetAllArticlesByTagIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAllArticlesByTagIdQuery, PaginatedList<ArticleBriefDto>>
{



    public async Task<PaginatedList<ArticleBriefDto>> Handle(GetAllArticlesByTagIdQuery request, CancellationToken cancellationToken)
    {
        var articles = await context.Articles
            .Include(a => a.Tags)
            .Where(a => a.Tags.Any(t => t.Id == request.TagId))
            .OrderByDescending(a => a.Id)
            .ProjectTo<ArticleBriefDto>(mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return articles;
    }
}
