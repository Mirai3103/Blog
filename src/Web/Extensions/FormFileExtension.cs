namespace Blog.Web.Extensions;

public static class FormFileExtension
{
    public static string CreateNewFileName(this IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        return $"{Guid.NewGuid()}";
    }
}
