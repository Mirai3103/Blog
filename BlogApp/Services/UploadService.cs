namespace BlogApp.Services;

public class UploadService
{
    public UploadService()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    public Task RemoveFile(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }
    public Task RemoveFiles(ICollection<string> fileNames)
    {
        var listTask = fileNames.Select(f =>
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", f);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return Task.CompletedTask;
        }).ToArray();
        return Task.WhenAll(listTask);
    }
    public ICollection<string> GetUploadedFileNames()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        return Directory.GetFiles(path).Select(f => f.Replace(path, "").Replace("\\", "")).ToList();
    }
    public string UploadFile(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return "/uploads/" + fileName;
    }

    public string UploadFile(IFormFile file, string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return "/uploads/" + fileName;
    }

    public ICollection<string> UploadFiles(ICollection<IFormFile> files)
    {
        var listTask = files.Select(f =>
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(files.First().FileName)}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                f.CopyToAsync(stream).GetAwaiter().GetResult();
            }

            return Task.FromResult("/uploads/" + fileName);
        }).ToArray();
        return Task.WhenAll(listTask).GetAwaiter().GetResult();
    }
}