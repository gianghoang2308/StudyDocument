using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;


namespace StudyDocument.ViewComponents.Image
{
    public class ImageDetailViewComponent : ViewComponent
    {
        private readonly StudyPlatform_BkapContext imgs = new StudyPlatform_BkapContext();
        public IViewComponentResult Invoke(int id)
        {
            var image = imgs.Images.Include(c => c.Comments).ThenInclude(u => u.User).FirstOrDefault(img => img.Id == id);

            if (image == null)
            {
                return View("NotFound");
            }

            string? uploaderName = null;
            string? uploaderAvatar = null;

            if (image.UserId != null)
            {
                var user = imgs.Users.FirstOrDefault(u => u.Id == image.UserId);
                if (user != null)
                {
                    uploaderName = user.FullName;
                    uploaderAvatar = user.Avatar;
                }
                else
                {
                    var admin = imgs.Admins.FirstOrDefault(a => a.Id == image.UserId);
                    if (admin != null)
                    {
                        uploaderName = admin.FullName;
                        uploaderAvatar = admin.Avatar;
                    }
                }
            }

            ViewBag.UploaderName = uploaderName ?? "Unknown";
            ViewBag.UserAvatar = string.IsNullOrEmpty(uploaderAvatar)
            ? image.UserId != null && imgs.Users.Any(u => u.Id == image.UserId)
                ? $"/uploads/users/{uploaderAvatar}"
                : $"/uploads/admins/{uploaderAvatar}"
            : uploaderAvatar;
            

            return View("Default", image);
        }
    }
}
