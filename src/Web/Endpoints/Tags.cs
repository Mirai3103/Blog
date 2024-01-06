using System.Security.Principal;
using Blog.Application.Common.Models;
using Blog.Application.Tags.Commands.CreateTag;
using Blog.Application.Tags.Commands.DeleteTag;
using Blog.Application.Tags.Commands.UpdateTag;
using Blog.Application.Tags.Queries.Dtos;
using Blog.Application.Tags.Queries.GetAllTags;
using Blog.Application.Tags.Queries.GetTagById;
using Blog.Application.Tags.Queries.GetTagBySlug;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Endpoints;

public class Tags : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {

        app.MapGroup(this)
            .MapGet(GetAllTags)
            .MapGet(GetTagByUniqueIdentifier, "{identifier}")
            
            .MapPost(CreateTag)
            .MapPut(UpdateTag, "{id}")
            .MapDelete(DeleteTag, "{id}");

    
    }

    public async Task<PaginatedList<TagDto>> GetAllTags(ISender sender, [AsParameters] GetAllTagsQuery query)
    {
        return await sender.Send(query);
    }

    [Authorize]
    public async Task<IResult> CreateTag(ISender sender, [FromBody] CreateTagCommand command)
    {
        var id = await sender.Send(command);
        return Results.CreatedAtRoute(nameof(GetTagByUniqueIdentifier), new { identifier = id }, null);
    }
    [Authorize]
    public async Task<IResult> UpdateTag(ISender sender, int id, UpdateTagCommand command)
    {
        command.Id = id;
        await sender.Send(command);
        return Results.NoContent();
    }

    [Authorize]
    public async Task<IResult> DeleteTag(ISender sender, int id)
    {
        await sender.Send(new DeleteTagCommand(id));
        // no content
        return Results.NoContent();
    }


    public async Task<TagDto> GetTagByUniqueIdentifier(ISender sender, [FromRoute] string identifier)
    {
        bool isNumeric = int.TryParse(identifier, out int n);
        return isNumeric ? await sender.Send(new GetTagByIdQuery(n)) : await sender.Send(new GetTagBySlugQuery(identifier));
    }
}
