
namespace BlogApp.Dto;

public class ListPostPaginationDto
{
    public int CurrentPage { get; set; }
    public int TotalPage { get; set; }
    public IEnumerable<BlogApp.Models.Post> Posts { get; set; } = null!;
}