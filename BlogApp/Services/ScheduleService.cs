

using Quartz;

namespace BlogApp.Services;

public class ScheduleService : IJob
{
    private readonly PostService _postService;
    private readonly UploadService _uploadService;

    public ScheduleService(PostService postService, UploadService uploadService)
    {
        _postService = postService;
        _uploadService = uploadService;
    }

    public void ClearUnusedImages()
    {
        Console.WriteLine("Clearing unused images");
        var listUploads = _uploadService.GetUploadedFileNames();
        foreach (var upload in listUploads)
        {
            var post = _postService.FindByThumbnailUrl(upload);
            if (post == null)
            {
                _uploadService.RemoveFile(upload).GetAwaiter().GetResult();
            }
        }
    }

    public Task Execute(IJobExecutionContext context)
    {
        ClearUnusedImages();
        return Task.CompletedTask;
    }
}