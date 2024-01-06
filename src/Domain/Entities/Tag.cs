namespace Blog.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
