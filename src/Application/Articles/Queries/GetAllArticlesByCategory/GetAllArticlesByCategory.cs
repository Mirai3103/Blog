using Blog.Application.Articles.Queries.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Mappings;
using Blog.Application.Common.Models;

namespace Blog.Application.Articles.Queries.GetAllArticlesByCategory;

public record GetAllArticlesByCategoryQuery(int CategoryId) : PaginationQuery, IRequest<PaginatedList<ArticleBriefDto>>
{
}

public class GetAllArticlesByCategoryQueryValidator : AbstractValidator<GetAllArticlesByCategoryQuery>
{
    public GetAllArticlesByCategoryQueryValidator()
    {
    }
}

public class GetAllArticlesByCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAllArticlesByCategoryQuery, PaginatedList<ArticleBriefDto>>
{
    public async Task<PaginatedList<ArticleBriefDto>> Handle(GetAllArticlesByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var articles = await context.Articles
            .Include(a => a.Tags)
            // .Where(a => a.Category.Id == request.CategoryId)
            .OrderByDescending(a => a.Id)
            .ProjectTo<ArticleBriefDto>(mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return articles;
    }
}
