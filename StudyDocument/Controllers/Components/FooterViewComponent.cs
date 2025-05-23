using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;

namespace Project_web.Controllers.Components
{
    [Authorize]
    [AllowAnonymous]
    public class FooterViewComponent : ViewComponent
    {
        private StudyPlatform_BkapContext footer = new StudyPlatform_BkapContext();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Config> configs = footer.Configs.ToList();
            ViewBag.Configs = configs;
            return View();
        }
    }
}
