using Blog.Application.Articles.Commands.CreateArticle;
using Blog.Application.Articles.Commands.DeleteArticle;
using Blog.Application.Articles.Commands.FullUpdateArticle;
using Blog.Application.Articles.Commands.PartialUpdateArticle;
using Blog.Application.Articles.Queries.Dtos;
using Blog.Application.Articles.Queries.GetAllArticle;
using Blog.Application.Articles.Queries.GetAllArticlesByCategory;
using Blog.Application.Articles.Queries.GetAllArticlesByTagId;
using Blog.Application.Articles.Queries.GetArticleById;
using Blog.Application.Articles.Queries.GetArticleBySlug;
using Blog.Application.Common.Models;
using Blog.Infrastructure.Services;
using Blog.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Endpoints;

public class Articles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAllArticles)
            .MapGet(GetArticleByUniqueIdentifier, "{identifier}")
            .MapGet(GetArticlesByCategory, "category/{categoryId}")
            .MapGet(GetArticlesByTag, "tag/{tagId}")
            .MapPost(CreateArticle)
            .MapPatch("{id}", UpdateArticle);
        app.MapGroup(this)
            .MapDelete("{id:int}", DeleteArticle);
    }

    public async Task<PaginatedList<ArticleBriefDto>> GetAllArticles(ISender sender,
        [AsParameters] GetAllArticleQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    public async Task<ArticleDto> GetArticleByUniqueIdentifier(ISender sender, [FromRoute] string identifier,UserService userService,
        CancellationToken cancellationToken)
    {
        bool isNumeric = int.TryParse(identifier, out int n);
        ArticleDto dto;
        if (isNumeric)
        {
            dto=await sender.Send(new GetArticleByIdQuery(n), cancellationToken);
        }
        else
        {
            dto =await sender.Send(new GetArticleBySlugQuery(identifier), cancellationToken);

        }

        dto.Author = (await userService.GetUserById(dto.CreatedBy!))!.MapToAuthorDto();
        return dto;

    }

    public async Task<PaginatedList<ArticleBriefDto>> GetArticlesByCategory(ISender sender, [FromRoute] int categoryId,
        CancellationToken cancellationToken)
    {
        return await sender.Send(new GetAllArticlesByCategoryQuery(categoryId), cancellationToken);
    }

    public async Task<PaginatedList<ArticleBriefDto>> GetArticlesByTag(ISender sender, [FromRoute] int tagId,
        CancellationToken cancellationToken,HttpContext context)
    {
        var   header = context.Request.Headers["Authorization"];
        return await sender.Send(new GetAllArticlesByTagIdQuery(tagId), cancellationToken);
    }
    [Authorize("CanCreateArticle")]
    public async Task<IResult> CreateArticle(ISender sender, [FromBody] CreateArticleCommand command,
        CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);
        return Results.CreatedAtRoute(nameof(GetArticleByUniqueIdentifier), new { identifier = id }, null);
    }

    public async Task<IResult> UpdateArticle(ISender sender, int id, HttpContext context,
        CancellationToken cancellationToken)
    {
        if (!context.Request.HasJsonContentType())
        {
            throw new BadHttpRequestException(
                "Request content type was not a recognized JSON content type.",
                StatusCodes.Status415UnsupportedMediaType);
        }

        using var sr = new StreamReader(context.Request.Body);
        var str = await sr.ReadToEndAsync(cancellationToken);
        var jsonPacth = JsonConvert.DeserializeObject<JsonPatchDocument<FullUpdateArticleCommand>>(str);
        if (jsonPacth == null)
        {
            throw new BadHttpRequestException(
                "Request content type was not a recognized JSON content type.",
                StatusCodes.Status415UnsupportedMediaType);
        }

        var command = new PartialUpdateArticleCommand();
        command.Id = id;
        command.Data = jsonPacth;
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteArticle(ISender sender, int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteArticleCommand(id), cancellationToken);
        // no content
        return Results.NoContent();
    }
}
