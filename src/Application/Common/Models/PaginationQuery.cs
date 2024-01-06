
using Blog.Domain.Entities;

namespace Blog.Application.Common.Models;

public abstract record PaginationQuery
{
    protected PaginationQuery()
    {
        PageNumber = 1;
        PageSize = 10;
        OrderByDescending = true;
    }

    public int PageNumber { get; init; } 
    public int PageSize { get; init; } 
    public string? Search { get; init; }
    public string? OrderBy { get; init; }
    public bool OrderByDescending { get; init; }
    
}

public abstract class AbstractPaginationQueryValidator<T> : AbstractValidator<T> where T : PaginationQuery
{
    protected AbstractPaginationQueryValidator()
    {
        RuleFor(v => v.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1);

            
    }
}
