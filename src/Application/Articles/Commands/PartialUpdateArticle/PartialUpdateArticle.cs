using Blog.Application.Articles.Commands.FullUpdateArticle;
using Blog.Application.Common.Interfaces;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Blog.Application.Articles.Commands.PartialUpdateArticle;


public class PartialUpdateArticleCommand : IRequest
{
    public JsonPatchDocument<FullUpdateArticleCommand> Data { get; set; } = null!;
    public int Id { get; set; }
}

public class PartialUpdateArticleCommandValidator : AbstractValidator<PartialUpdateArticleCommand>
{
    private readonly IApplicationDbContext _context;

    public PartialUpdateArticleCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Data)
            .NotNull().WithMessage("Operations không được để trống.")
            .NotEmpty().WithMessage("Operations không được để trống.");
        
        RuleFor(v => v.Id)
            .GreaterThan(0)
            .WithMessage("Id không hợp lệ.");

    }
}

public class UpdateArticleCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<PartialUpdateArticleCommand>
{


    public async Task
        Handle(PartialUpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await context.Articles.FindAsync(request.Id);
        if (article == null)
        {
            throw new NotFoundException( "Bài viết với id " + request.Id + " không tồn tại.", "Id");
        }
        
        var articleCommand = mapper.Map<FullUpdateArticleCommand>(article);
        request.Data.ApplyTo(articleCommand);
        var validator = new FullUpdateArticleCommandValidator(context);
        var result = await validator.ValidateAsync(articleCommand, cancellationToken);
        if (!result.IsValid)
        {   
            throw new ValidationException(result.Errors);
        }
        mapper.Map<FullUpdateArticleCommand,Article>(articleCommand, article);
        await context.SaveChangesAsync(cancellationToken);
    }
}
