using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Project_web.Controllers.Components
{
    [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
    public class HeaderViewComponent : ViewComponent
    {
        private StudyPlatform_BkapContext header = new StudyPlatform_BkapContext();
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            return View();
        }
    }
    
}
