namespace BlogApp.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
public class UploadService
{
    private IConfiguration Configuration { get; }
    private readonly Cloudinary _cloudinary;
    public UploadService(IConfiguration configuration)
    {
        Configuration = configuration;
        var cloudname = Configuration["Cloudinary:CloudName"] ?? Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
        var apikey = Configuration["Cloudinary:ApiKey"] ?? Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
        var apisecret = Configuration["Cloudinary:ApiSecret"] ?? Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");
        var account = new Account(cloudname, apikey, apisecret);
        _cloudinary = new Cloudinary(account);
        // var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        // if (!Directory.Exists(path))
        // {
        //     Directory.CreateDirectory(path);
        // }
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
    // public string UploadFile(IFormFile file)
    // {
    //     var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
    //     var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
    //     using (var stream = new FileStream(path, FileMode.Create))
    //     {
    //         file.CopyTo(stream);
    //     }

    //     return "/uploads/" + fileName;
    // }

    public string UploadFile(IFormFile file, string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return "/uploads/" + fileName;
    }
    public string UploadFile(IFormFile file)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
        };
        var uploadResult = _cloudinary.Upload(uploadParams);
        return uploadResult.SecureUrl.AbsoluteUri;
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