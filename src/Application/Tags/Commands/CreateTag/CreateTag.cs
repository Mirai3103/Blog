using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Interfaces;
using Blog.Domain.Entities;

namespace Blog.Application.Tags.Commands.CreateTag;

public record CreateTagCommand()
    : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
}

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandValidator(IApplicationDbContext context)
    {
        this. _context = context;
        RuleFor(v => v.Name)
            .MaximumLength(20).WithMessage("Tên không được vượt quá 20 ký tự.")
            .NotNull().WithMessage("Tên không được để trống.")
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MustAsync(BeUniqueName).WithMessage("Tên đã tồn tại.")
            .WithName("Name");

        RuleFor(v => v.Slug)
            .MaximumLength(20).WithMessage("Slug không được vượt quá 20 ký tự.")
            .NotNull().WithMessage("Slug không được để trống.")
            .NotEmpty().WithMessage("Slug không được để trống.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug không hợp lệ.")
            .MustAsync(BeUniqueSlug).WithMessage("Slug đã tồn tại.")
            .WithName("Slug");
    }
    public async Task<bool>  BeUniqueSlug(string slug, CancellationToken cancellationToken)
    {
        return await _context.Tags.AllAsync(x => x.Slug != slug, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Tags.AllAsync(x => x.Name != name, cancellationToken);
    }
}

public class CreateTagCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTagCommand, int>
{
    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = new Tag { Name = request.Name, Slug = request.Slug };
        var existsWithSlug = await context.Tags.AnyAsync(x => x.Slug == entity.Slug, cancellationToken);
        var existsWithName = await context.Tags.AnyAsync(x => x.Name == entity.Name, cancellationToken);
        if (existsWithSlug)
        {
            throw new ConflictException($"Tag with slug '{entity.Slug}' already exists");
        }

        if (existsWithName)
        {
            throw new ConflictException($"Tag with name '{entity.Name}' already exists");
        }

        context.Tags.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
