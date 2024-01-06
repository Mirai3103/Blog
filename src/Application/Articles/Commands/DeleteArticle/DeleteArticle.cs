using Blog.Application.Common.Interfaces;

namespace Blog.Application.Articles.Commands.DeleteArticle;

public record DeleteArticleCommand(int Id) : IRequest
{
}

public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
    }
}

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteArticleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await _context.Articles.FindAsync(request.Id);
        if (article == null)
        {
            throw new NotFoundException("Bài viết với id " + request.Id + " không tồn tại.", "Id");
        }

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
