using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Views.Shared.Components.PortCard
{
    [ViewComponent(Name = "PostCard")]
    public class PostCard : ViewComponent
    {
        public IViewComponentResult Invoke(Post post)
        {
            return View(post);
        }
    }
}
