using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Views.Shared.Components.Badge
{

    public struct BadgeProps
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
    [ViewComponent(Name = "Badge")]
    public class Badge : ViewComponent
    {

        public IViewComponentResult Invoke(BadgeProps props)
        {

            return View(props);

        }
    }
}
