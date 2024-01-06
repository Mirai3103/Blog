using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Interfaces;

namespace Blog.Application.Tags.Commands.UpdateTag;

public record UpdateTagCommand
    : IRequest
{
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int Id { get; set; }
}

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
         RuleFor(v => v.Name)
            .MaximumLength(20).WithMessage("Tên không được vượt quá 20 ký tự.")
            .NotNull().WithMessage("Tên không được để trống.")
            .NotEmpty().WithMessage("Tên không được để trống.");

        RuleFor(v => v.Slug)
            .MaximumLength(20).WithMessage("Slug không được vượt quá 20 ký tự.")
            .NotNull().WithMessage("Slug không được để trống.")
            .NotEmpty().WithMessage("Slug không được để trống.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug không hợp lệ.");
    }
}

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tags = await _context.Tags.FindAsync(request.Id, cancellationToken);
        if (tags == null)
        {
            return;
        }

        var existsWithSlugTask = 
          await  _context.Tags.AnyAsync(x => x.Slug == request.Slug && x.Id != request.Id, cancellationToken);
        var existsWithName =
         await   _context.Tags.AnyAsync(x => x.Name == request.Name && x.Id != request.Id, cancellationToken);
        if ( existsWithSlugTask)
        {
            throw new ConflictException($"Tag with slug '{request.Slug}' already exists");
        }

        if ( existsWithName)
        {
            throw new ConflictException($"Tag with name '{request.Name}' already exists");
        }

        tags.Name = request.Name;
        tags.Slug = request.Slug;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
