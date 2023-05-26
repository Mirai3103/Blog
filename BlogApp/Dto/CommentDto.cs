namespace BlogApp.Dto;

public class CommentDto
{
    public string Content { get; set; } = null!;
    public int PostId { get; set; }
    public int UserId { get; set; }
}