using Blog.Application.Common.Interfaces;

namespace Blog.Application.Tags.Commands.DeleteTag;

public record DeleteTagCommand (int TagId): IRequest
{
}

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(v => v.TagId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

    }
}

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags.FindAsync(request.TagId, cancellationToken);
        if (tags == null)
        {
            return;
        }
        _context.Tags.Remove(tags);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
