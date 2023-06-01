using BlogApp.DAL;
using BlogApp.Dto;
using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostService _postService;
        private readonly BlogContext _context;
        private readonly UploadService _uploadService;

        public HomeController(ILogger<HomeController> logger, PostService postService, BlogContext context, UploadService uploadService)
        {
            _logger = logger;
            _postService = postService;
            _context = context;
            _uploadService = uploadService;
        }

        public IActionResult Index([FromQuery(Name = "page")] int page = 1)
        {
            var posts = _postService.GetPosts(page);

            return View(posts);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("post/{slug}")]
        public IActionResult PostDetail(string slug)
        {
            var post = _postService.GetPostBySlug(slug, true);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [Authorize(Roles = nameof(Models.User.Role.ADMIN) + "," + nameof(Models.User.Role.CREATOR))]
        [HttpGet("post/create")]
        public IActionResult CreatePost()
        {

            return View();
        }
        [Authorize(Roles = nameof(Models.User.Role.ADMIN) + "," + nameof(Models.User.Role.CREATOR))]
        [HttpPost("post/delete/{id}")]
        public IActionResult DeletePost(string id)
        {

            var post = _postService.FindById(int.Parse(id));
            if (post is null)
            {
                return NotFound();
            }
            var userRole = User.FindFirst(ClaimTypes.Role).Value;
            if (userRole == nameof(Models.User.Role.CREATOR) && post.Author.Id.ToString() != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            TempData["Success"] = "Xóa bài viết thành công";
            _postService.DeletePost(int.Parse(id));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("post/edit/{slug}")]
        public IActionResult EditPost(string slug)
        {

            var post = _postService.GetPostBySlug(slug);
            if (post is null)
            {
                return NotFound();
            }

            return View(new EditPostDto()
            {
                AuthorId = post.Author.Id,
                Content = post.Content,
                Id = post.Id,
                Summary = post.Summary,
                ThumbnailUrl = post.ThumbnailUrl,
                Title = post.Title

            });
        }
        [HttpPost("post/edit/{slug}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(EditPostDto post, [FromForm(Name = "thumbnail")] IFormFile thumbnail, [FromRoute] string slug)
        {
            if (thumbnail is not null)
            {
                var fileName = _uploadService.UploadFile(thumbnail);
                post.ThumbnailUrl = fileName;
            }
            else
            {
                post.ThumbnailUrl = "";
            }

            ModelState.Remove("ThumbnailUrl");
            ModelState.Remove("thumbnail");
            if (ModelState.IsValid)
            {
                _postService.EditPost(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }
        [HttpPost("post/create")]

        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(CreatePostDto post, [FromForm(Name = "thumbnail")] IFormFile thumbnail)
        {
            var fileName = _uploadService.UploadFile(thumbnail);
            post.ThumbnailUrl = fileName;
            ModelState.Remove("ThumbnailUrl");
            if (ModelState.IsValid)
            {
                _postService.CreatePost(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

    }
}