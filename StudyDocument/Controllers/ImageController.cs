using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using X.PagedList.Extensions;


namespace StudyDocumentPlatform.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
    public class ImageController : Controller
    {
       private readonly StudyPlatform_BkapContext imgs = new StudyPlatform_BkapContext();

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        public IActionResult Index(string imageName, bool? active, DateTime? timeRange, int pageNumber = 1)
        {
            var query = imgs.Images.AsQueryable();

            ViewBag.ImageList = imgs.Images.Select(i => new { i.Id, i.Name }).ToList();

            if (!string.IsNullOrEmpty(imageName))
            {
                query = query.Where(v => v.Name.Contains(imageName));
            }

            if (active.HasValue)
            {
                query = query.Where(v => v.Status == active.Value);
            }

            if (timeRange.HasValue)
            {
                var filterDate = timeRange.Value.Date;
                query = query.Where(v => v.CreateTime.Date == filterDate);
                ViewBag.TimeRange = timeRange.Value.ToString("yyyy-MM-dd");
            }

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var pageSize = 10;
            var pageList = query.OrderBy(v => v.Id).ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        // GET: Detail
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var comment = imgs.Comments.FirstOrDefault(c => c.Id == id) ?? new Comment { Id = id };
            return View(comment);

        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        public IActionResult CreateComment(Comment data, int id)
        {
            var userId = User.Identity.Name;

            var user = imgs.Users.FirstOrDefault(u => u.UserName == userId);
            if (user == null)
            {
                return View("NotFound");
            }

            data.UserId = user.Id;
            data.Date = DateTime.Now;
            data.DateOfBirth = null;
            data.Status = true;

            imgs.Add(data);
            imgs.SaveChanges();

            return RedirectToAction("Detail");
        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> cat = imgs.Categories.ToList();
            ViewBag.CategoryList = cat;
            return View();
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        [RequestSizeLimit(500L * 1024 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 500L * 1024 * 1024 * 1024)]
        public IActionResult Create(Image data)
        {
            List<Category> cat = imgs.Categories.ToList();
            ViewBag.CategoryList = cat;
            data.CreateTime = DateTime.Now;
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                data.Image1 = fileName;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    data.ImageData = memoryStream.ToArray();
                    data.ImageType = file.ContentType;
                    data.Link = "";
                }
            }

            else if (!string.IsNullOrEmpty(data.Link))
            {
                data.Image1 = "Link";
                data.ImageData = new byte[0];
                data.ImageType = "Link From Internet";
                data.Link = "";
            }

            else
            {
                ModelState.AddModelError("", "Vui lòng nhập Link ảnh hoặc chọn file ảnh.");
                return View(data);
            }

            imgs.Images.Add(data);
            imgs.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        public IActionResult Edit(int id)
        {
       
            var image = imgs.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }
            else
            {
                List<Category> cat = imgs.Categories.ToList();
                ViewBag.CategoryList = cat;


                return View(image);
            }
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        public IActionResult Edit(Image data, string oldImage, string oldLink, string oldImageType)
        {
            ViewBag.CategoryList = imgs.Categories.ToList();

            var file = Request.Form.Files.FirstOrDefault();

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                data.Image1 = fileName;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    data.ImageData = memoryStream.ToArray();
                    data.ImageType = file.ContentType;
                }

                data.Link = "";
            }
            else if (!string.IsNullOrEmpty(data.Link) && data.Image1 == "Link")
            {

                data.ImageData = new byte[0];
                data.ImageType = "Link From Internet";
            }
            else
            {
                var existing = imgs.Images.AsNoTracking().FirstOrDefault(x => x.Id == data.Id);
                if (existing != null)
                {
                    data.Image1 = oldImage;
                    data.Link = oldLink;
                    data.ImageData = existing.ImageData;
                    data.ImageType = oldImageType;
                }
            }
            imgs.Attach(data);
            imgs.Entry(data).State = EntityState.Modified;
            imgs.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var image = imgs.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            imgs.Images.Remove(image);
            imgs.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult GellAllImage(int pageNumber = 1)
        {
            var pageSize = 6;
            var pagedImages = imgs.Images.OrderBy(i => i.Id).ToPagedList(pageNumber, pageSize);
            var recentImages = imgs.Images.OrderByDescending(i => i.CreateTime).Take(4).ToList();

            ViewBag.PagedImages = pagedImages;
            ViewBag.RecentImages = recentImages;

            return View();
        }

    }
}
