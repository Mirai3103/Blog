using Blog.Application.Articles.Queries.Dtos;
using Blog.Application.Common.Interfaces;

namespace Blog.Application.Articles.Queries.GetArticleBySlug;

public record GetArticleBySlugQuery(String Slug) : IRequest<ArticleDto>
{
}

public class GetArticleBySlugQueryValidator : AbstractValidator<GetArticleBySlugQuery>
{
    public GetArticleBySlugQueryValidator()
    {
    }
}

public class GetArticleBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetArticleBySlugQuery, ArticleDto>
{
    public async Task<ArticleDto> Handle(GetArticleBySlugQuery request, CancellationToken cancellationToken)
    {
        var article = await context.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);
        
        if (article is null)
            throw new NotFoundException("Article", request.Slug);
        return mapper.Map<ArticleDto>(article);
    }
}
