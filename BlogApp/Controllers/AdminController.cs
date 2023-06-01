namespace BlogApp.Controllers;
using BlogApp.DAL;
using BlogApp.Dto;
using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;
[Authorize(Roles = nameof(Models.User.Role.ADMIN) + "," + nameof(Models.User.Role.CREATOR))]

public class AdminController : Controller
{

    private readonly PostService _postService;
    public AdminController(PostService postService)
    {
        _postService = postService;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("[Controller]/[action]")]
    public IActionResult PostIndex([FromQuery(Name = "page")] int page = 1)
    {
        var posts = _postService.GetPosts(page);

        return View(posts);
    }

}