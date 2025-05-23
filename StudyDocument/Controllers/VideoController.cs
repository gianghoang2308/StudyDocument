using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;
using X.PagedList.Extensions;


namespace StudyDocumentPlatform.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
    public class VideoController : Controller
    {
        private StudyPlatform_BkapContext vds = new StudyPlatform_BkapContext(); // Your Entity Framework context

        // GET: Video/Create
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Index(string description, bool? active, DateTime? timeRange, int pageNumber = 1)
        {
            var query = vds.Videos.Include(v => v.Category).AsQueryable();
            ViewBag.CategoryList = vds.Categories.ToList();

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(v => v.Description.Contains(description));
                ViewBag.DescriptionFilter = description; // Lưu lại để đổ vào form filter
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
            var pageList = query.OrderBy(vd => vd.Id).ToPagedList(pageNumber, pageSize);

            return View(pageList);
        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> cat = vds.Categories.ToList();
            ViewBag.CategoryList = cat;
            return View();
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        [RequestSizeLimit(500L * 1024 * 1024 * 1024)] 
        [RequestFormLimits(MultipartBodyLengthLimit = 500L * 1024 * 1024 * 1024)]
        public IActionResult Create(Video data)
        {
            data.CreateTime = DateTime.Now;
            data.Thumbnail = "";
            data.Name = "";
            data.UserId = 1;
            data.Views = 0;
            data.Status = true;
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");

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
                data.Video1 = fileName;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    data.VideoData = memoryStream.ToArray();
                    data.VideoType = file.ContentType;
                    data.Link = "";
                }
            }
            else if (!string.IsNullOrEmpty(data.Link))
            {
                data.Video1 = "Link";
                data.VideoData = new byte[0];
                data.VideoType = "Link From Internet";
                data.Link = data.Link;
            }

            else
            {
                ModelState.AddModelError("", "Vui lòng nhập Link video hoặc chọn file video.");
                return View(data);
            }
            vds.Add(data);
            vds.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var existingVideo = vds.Videos.FirstOrDefault(v => v.Id == id);
            if (existingVideo == null)
            {
                return RedirectToAction("Index");
            }
            List<Category> cat = vds.Categories.ToList();
            ViewBag.CategoryList = cat;

            return View(existingVideo);
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        public IActionResult Edit(Video data, string oldVideo, string oldLink, string oldVideoType)
        {
            data.Thumbnail = "";
            data.Name = "";
            data.CreateTime = DateTime.Now;
            data.UserId = 1;
            data.Views = 0;
            ViewBag.CategoryList = vds.Categories.ToList();

            var file = Request.Form.Files.FirstOrDefault();

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                data.Video1 = fileName;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    data.VideoData = memoryStream.ToArray();
                    data.VideoType = file.ContentType;
                    data.Link = "";
                }

                data.Link = "";
            }
            else if (!string.IsNullOrEmpty(data.Link))
            {
                data.Video1 = "Link";
                data.VideoData = new byte[0];
                data.VideoType = "Link From Internet";
            }
            else
            {
                var existing = vds.Videos.AsNoTracking().FirstOrDefault(x => x.Id == data.Id);
                if (existing != null)
                {
                    data.Video1 = oldVideo;
                    data.Link = "";
                    data.VideoData = existing.VideoData;
                    data.VideoType = oldVideoType;
                }
            }

            vds.Attach(data);
            vds.Entry(data).State = EntityState.Modified;
            vds.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var vd = vds.Videos.Find(id);
            if (vd == null)
            {
                return NotFound($"Can not find the document with id: {id}");
            }
            else
            {
                vds.Videos.Remove(vd);
                vds.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult GetAllVideo(int pageNumber = 1)
        {
            var pageSize = 6;
            var pagedVideos = vds.Videos.OrderBy(i => i.Id).ToPagedList(pageNumber, pageSize);
            var recentVideos = vds.Videos.OrderByDescending(i => i.CreateTime).Take(4).ToList();

            ViewBag.PagedVideos = pagedVideos;
            ViewBag.RecentVideos = recentVideos;

            return View();
        }
    }
}
