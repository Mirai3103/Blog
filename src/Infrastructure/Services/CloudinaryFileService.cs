using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Services;

public class CloudinaryOptions
{
    public string CloudName { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
    public string ApiSecret { get; set; } = null!;
}
public class CloudinaryFileService:IFileService
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryFileService(IConfiguration configuration)
    {
        var cloudinaryOptions = configuration.GetSection("Cloudinary").Get<CloudinaryOptions>()??throw new ArgumentNullException();
       var account = new Account(cloudinaryOptions.CloudName, cloudinaryOptions.ApiKey, cloudinaryOptions.ApiSecret);
       _cloudinary = new Cloudinary(account);
    }
    public async Task<string> SaveFileAsync(Stream fileStream)
    {
        var id = Guid.NewGuid();
     
     var a =await   _cloudinary.UploadAsync(new CloudinaryDotNet.Actions.ImageUploadParams()
        {
            File = new FileDescription(id.ToString(), fileStream),
            PublicId = id.ToString(),
            Overwrite = true,
            UseFilename = true,
            Folder = "blog"
        });
        return  a.SecureUrl.AbsoluteUri;
    }

    public Task DeleteFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}
