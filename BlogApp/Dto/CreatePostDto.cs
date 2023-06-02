namespace BlogApp.Dto;

public class CreatePostDto
{

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    public string[] Tags { get; set; } = null!;
}