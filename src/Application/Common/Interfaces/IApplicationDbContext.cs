using Blog.Domain.Entities;

namespace Blog.Application.Common.Interfaces;

public interface IApplicationDbContext
{

    DbSet<Tag> Tags { get; }
    DbSet<Article> Articles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
