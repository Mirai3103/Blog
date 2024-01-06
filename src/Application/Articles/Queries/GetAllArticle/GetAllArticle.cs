using Blog.Application.Articles.Queries.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Mappings;
using Blog.Application.Common.Models;

namespace Blog.Application.Articles.Queries.GetAllArticle;

public record GetAllArticleQuery :PaginationQuery, IRequest<PaginatedList<ArticleBriefDto>>
{
}

public class GetAllArticleQueryValidator : AbstractValidator<GetAllArticleQuery>
{
    public GetAllArticleQueryValidator()
    {
    }
}

public class GetAllArticleQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAllArticleQuery, PaginatedList<ArticleBriefDto>>
{
  

    public async Task<PaginatedList<ArticleBriefDto>> Handle(GetAllArticleQuery request, CancellationToken cancellationToken)
    {
        var allArticles = await context.Articles.ToListAsync();
       var articles = await context.Articles
            .Include(a => a.Tags)
            .OrderByDescending(a => a.Id)
            .ProjectTo<ArticleBriefDto>(mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return articles;
    }
}
