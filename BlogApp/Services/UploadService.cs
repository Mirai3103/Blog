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

}