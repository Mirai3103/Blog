using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BlogApp.DAL;
using BlogApp.Services;
using System.Linq;
using BlogApp.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostService _postService;
        private readonly BlogContext _context;
        private readonly UploadService _uploadService;

        public HomeController(ILogger<HomeController> logger, PostService postService , BlogContext context, UploadService uploadService)
        {
            _logger = logger;
            _postService = postService;
            _context = context;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            var posts  = _postService.GetPosts();
                
            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("post/{slug}")]
        public IActionResult PostDetail(string slug)
        {
            var post = _postService.GetPostBySlug(slug);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpGet("post/create")]
        public IActionResult CreatePost()
        {
            
                ViewBag.AuthorId = new SelectList(_context.Users.Select(p=>new {p.Id,p.DisplayName}).ToList(), "Id", "DisplayName");
           

            return View();
        }
        [HttpPost("post/create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost( CreatePostDto post,[FromForm(Name ="thumbnail")] IFormFile thumbnail)
        {
            var fileName = _uploadService.UploadFile(thumbnail);
            post.ThumbnailUrl = fileName;
            ModelState.Remove("ThumbnailUrl");
            if (ModelState.IsValid)
            {
                _postService.CreatePost(post);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AuthorId = new SelectList(_context.Users.Select(p=>new {p.Id,p.DisplayName}).ToList(), "Id", "DisplayName");
            return View(post);
        }
    }
}