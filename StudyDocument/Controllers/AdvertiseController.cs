using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using X.PagedList.Extensions;

namespace StudyDocument.Controllers
{
    [Authorize(Roles = "AdminRole")]
    public class AdvertiseController : Controller
    {
        private readonly StudyPlatform_BkapContext ads = new StudyPlatform_BkapContext();

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            if(pageNumber <1 )
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = ads.Advertises.OrderBy(ad => ad.Id).ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var ad = ads.Advertises.Find(id);
            if(ad == null)
            {
                return NotFound($"Can not find the advertise with id: {id}!");
            }
            return View(ad);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        public IActionResult Create(Advertise data)
        {
            data.Position = 0;
            data.CreateTime = DateTime.Now;
            ads.Advertises.Add(data);
            ads.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ad = ads.Advertises.Find(id);
            if(ad == null)
            {
                return NotFound($"Can not find  the advertise with id: {id}");
            }
            return View(ad);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Edit(Advertise data)
        {
            data.Position = 0;
            data.CreateTime = DateTime.Now;
            ads.Advertises.Update(data);
            ads.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var ad = ads.Advertises.Find(id);
            if(ad == null)
            {
                return NotFound($"Can not find the advertise with id: {id}");
            }
            ads.Advertises.Remove(ad);
            ads.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
