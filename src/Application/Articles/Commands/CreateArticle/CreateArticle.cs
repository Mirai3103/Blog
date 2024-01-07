using Blog.Application.Common.Interfaces;
using Blog.Domain.Entities;

namespace Blog.Application.Articles.Commands.CreateArticle;

public record CreateArticleCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? ShortDescription { get; init; }

    public string DisplayImage { get; init; } = null!;

    public string Content { get; init; } = null!;

    public int CategoryId { get; init; }
    public ICollection<int> TagIds { get; init; } = new List<int>();
    public string? MetaDescription { get; init; }
}

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateArticleCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Title)
            .NotNull().WithMessage("Tên không được để trống.")
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MustAsync(BeUniqueName).WithMessage("Tên đã tồn tại.")
            .WithName("Title");

        RuleFor(v => v.Slug)
            .NotNull().WithMessage("Slug không được để trống.")
            .NotEmpty().WithMessage("Slug không được để trống.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug không hợp lệ.")
            .MustAsync(BeUniqueSlug).WithMessage("Slug đã tồn tại.")
            .WithName("Slug");

        RuleFor(v => v.ShortDescription)
            .MaximumLength(500).WithMessage("Mô tả ngắn không được vượt quá 500 ký tự.");

        RuleFor(v => v.DisplayImage)
            .NotNull().WithMessage("Ảnh không được để trống.")
            .NotEmpty().WithMessage("Ảnh không được để trống.");

        RuleFor(v => v.Content)
            .NotNull().WithMessage("Nội dung không được để trống.")
            .NotEmpty().WithMessage("Nội dung không được để trống.");

    }
    
    public async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    {
        return await _context.Tags.AllAsync(x => x.Slug != slug, cancellationToken);
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Tags.AllAsync(x => x.Name != name, cancellationToken);
    }
}

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateArticleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Article()
        {
            Title = request.Title,
            Slug = request.Slug,
            ShortDescription = request.ShortDescription,
            DisplayImage = request.DisplayImage,
            Content = request.Content,
            MetaDescription = request.MetaDescription
        };
        if (request.TagIds.Count > 0)
        {
            entity.Tags = await _context.Tags.Where(x => request.TagIds.Contains(x.Id)).ToListAsync(cancellationToken);
        }
        _context.Articles.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
