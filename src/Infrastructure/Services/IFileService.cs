namespace Blog.Infrastructure.Services;

public interface IFileService
{
    Task<string> SaveFileAsync( Stream fileStream);
    Task DeleteFileAsync(string fileName);
}
