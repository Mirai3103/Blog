

using Microsoft.AspNetCore.Mvc;
namespace BlogApp.Views.Shared.Components.NotificationToast
{
    public enum NotificationToastType
    {
        Success,
        Error,
        Warning,
        Info
    }

    [ViewComponent(Name = "NotificationToast")]
    public class NotificationToast : ViewComponent
    {

        public IViewComponentResult Invoke(dynamic props)
        {
            return View(props);
        }
    }
}