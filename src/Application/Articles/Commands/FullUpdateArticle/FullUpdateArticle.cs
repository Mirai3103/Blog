using Blog.Application.Common.Interfaces;
using Blog.Domain.Entities;

namespace Blog.Application.Articles.Commands.FullUpdateArticle;



public record FullUpdateArticleCommand : IRequest
{
    public string Title { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string? ShortDescription { get; init; }

    public string DisplayImage { get; init; } = null!;

    public string Content { get; init; } = null!;

    public int CategoryId { get; init; }
    public ICollection<int> TagIds { get; init; } = new List<int>();
    public string? MetaDescription { get; init; }
    public int Id { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Article, FullUpdateArticleCommand>()
                .ForMember(d => d.TagIds, opt => opt.MapFrom(s => s.Tags.Select(x => x.Id)));
            
            CreateMap<FullUpdateArticleCommand, Article>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.TagIds.Select(x => new Tag {Id = x})));
        }
    }
}

public class FullUpdateArticleCommandValidator : AbstractValidator<FullUpdateArticleCommand>
{
    private readonly IApplicationDbContext _context;

    public FullUpdateArticleCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Title)
            .NotNull().WithMessage("Tên không được để trống.")
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MustAsync((x, y, z) => BeUniqueName(x.Id, y, z))
            .WithMessage("Tên đã tồn tại.")
            .WithName("Title");

        RuleFor(v => v.Slug)
            .NotNull().WithMessage("Slug không được để trống.")
            .NotEmpty().WithMessage("Slug không được để trống.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug không hợp lệ.")
            .MustAsync((x, y, z) => BeUniqueSlug(x.Id, y, z))
            .WithMessage("Slug đã tồn tại.")
            .WithName("Slug");

        RuleFor(v => v.ShortDescription)
            .MaximumLength(200).WithMessage("Mô tả ngắn không được vượt quá 200 ký tự.");

        RuleFor(v => v.DisplayImage)
            .NotNull().WithMessage("Ảnh không được để trống.")
            .NotEmpty().WithMessage("Ảnh không được để trống.");

        RuleFor(v => v.Content)
            .NotNull().WithMessage("Nội dung không được để trống.")
            .NotEmpty().WithMessage("Nội dung không được để trống.");
    }

    private async Task<bool> BeUniqueSlug(int id, string slug, CancellationToken cancellationToken)
    {
        return await _context.Tags.AllAsync(x => x.Slug != slug || x.Id == id, cancellationToken);
    }

    private async Task<bool> BeUniqueName(int id, string name, CancellationToken cancellationToken)
    {
        return await _context.Tags.AllAsync(x => x.Name != name || x.Id == id, cancellationToken);
    }
}

