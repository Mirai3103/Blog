using Blog.Application.Common.Exceptions;
using Blog.Infrastructure.Services;
using Blog.Web.Extensions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace Blog.Web.Endpoints;

public class Files : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(UploadFile);
    }
    [Authorize]
    // custom documentation for swagger
    [OpenApiOperation("UploadFile", "Uploads a file")]
    [OpenApiBodyParameter("file")]
    public async Task<IResult> UploadFile(HttpContext context, IFileService fileService)
    {
        var file = context.Request.Form.Files[0];
        if (file.Length > 2 * 1024 * 1024)
        {
            return Results.BadRequest("File size is too large");
        }
        var stream = file.OpenReadStream();
        var url = await fileService.SaveFileAsync(stream);
        return Results.Ok(url);
    }
}
