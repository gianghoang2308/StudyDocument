using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;

namespace StudyDocument.ViewComponents.Image
{
    public class RecentlyPostingImagesViewComponent : ViewComponent
    {
        private readonly StudyPlatform_BkapContext imgs = new StudyPlatform_BkapContext();

        public IViewComponentResult Invoke()
        {
            var recentImages = imgs.Images.OrderByDescending(i => i.CreateTime).Take(4).ToList();
            return View(recentImages);

        }
    }
}
