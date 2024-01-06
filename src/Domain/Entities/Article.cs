namespace Blog.Domain.Entities;

public class Article : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ShortDescription { get; set; } 

    public string DisplayImage { get; set; } = null!;

    public string Content { get; set; } = null!;

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public string? MetaDescription { get; set; }

    
  
}
