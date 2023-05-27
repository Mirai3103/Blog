using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace BlogApp.Views.Shared.Components.CarouserItem
{
    [ViewComponent(Name = "CarouserItem")]
    public class CarouserItem : ViewComponent
    {
        public IViewComponentResult Invoke(Post post)
        {
            return View(post);
        }
    }
}