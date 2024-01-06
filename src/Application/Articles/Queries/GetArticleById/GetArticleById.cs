using Blog.Application.Articles.Queries.Dtos;
using Blog.Application.Common.Interfaces;

namespace Blog.Application.Articles.Queries.GetArticleById;

public record GetArticleByIdQuery(int id) : IRequest<ArticleDto>
{
}

public class GetArticleByIdQueryValidator : AbstractValidator<GetArticleByIdQuery>
{
    public GetArticleByIdQueryValidator()
    {
    }
}

public class GetArticleByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetArticleByIdQuery, ArticleDto>
{
    public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await context.Articles
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == request.id, cancellationToken);

        if (article is null)
            throw new NotFoundException("Article", request.id.ToString());

        return mapper.Map<ArticleDto>(article);
    }
}
