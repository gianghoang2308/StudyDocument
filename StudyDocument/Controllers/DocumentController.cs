using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;
using System.Web;
using X.PagedList.Extensions;
using System.IO;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;
using Microsoft.AspNetCore.Authorization;




namespace StudyDocumentPlatform.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
    public class DocumentController : Controller
    {
        private readonly StudyPlatform_BkapContext dcms = new StudyPlatform_BkapContext();

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Index(string description, bool? active, DateTime? timeRange, int pageNumber = 1)
        {
            var query = dcms.Documents.Include(v => v.Category).AsQueryable();
            ViewBag.CategoryList = dcms.Categories.ToList();

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
        public IActionResult Detail(int id)
        {
            var dcm = dcms.Documents.Find(id);
            if(dcm == null)
            {
                return NotFound($"Can not fint the document with id: {id}");
            }
            return View(dcm);
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Category> cat = dcms.Categories.ToList();
            ViewBag.CategoryList = cat;
            return View();
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        [RequestSizeLimit(500L * 1024 * 1024 * 1024)] // 500GB
        [RequestFormLimits(MultipartBodyLengthLimit = 500L * 1024 * 1024 * 1024)] // 500GB
        public IActionResult Create(Document data)
        {

            List<Category> cat_id = dcms.Categories.ToList();
            ViewBag.CategoryList = cat_id;
            data.CreateTime = DateTime.Now;
            data.UserId = 1;
            data.Status = true;
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");

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
                data.File = fileName;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    data.FileData = memoryStream.ToArray();
                    data.FileType = file.ContentType;
                }
            }

            dcms.Documents.Add(data);
            dcms.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            List<Category> cat_id = dcms.Categories.ToList();
            ViewBag.CategoryList = cat_id;
            var dcm = dcms.Documents.Find(id);
            return View(dcm);

        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        [RequestSizeLimit(500L * 1024 * 1024 * 1024)] // 500GB
        [RequestFormLimits(MultipartBodyLengthLimit = 500L * 1024 * 1024 * 1024)] // 500GB
        public IActionResult Edit( Document data, string oldFile)
        {
            data.CreateTime = DateTime.Now;
            data.UserId = 1;
            if (data.File == null)
            {
                data.File = oldFile;
            }

            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");

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
                data.File = fileName;

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    data.FileData = memoryStream.ToArray();
                    data.FileType = file.ContentType;
                }
            }
            else
            {
                
                var existingDocument = dcms.Documents.AsNoTracking().FirstOrDefault(d => d.Id == data.Id);

                if (existingDocument != null)
                {
                    data.FileData = existingDocument.FileData;
                    data.FileType = existingDocument.FileType;
                }
            }
            dcms.Update(data);
                dcms.SaveChanges();
                return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dcm = dcms.Documents.Find(id);
            if (dcm == null)
            {
                return NotFound($"Can not find the document with id: {id}");
            }
            dcms.Documents.Remove(dcm);
            dcms.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Download(int id)
        {
            var document = dcms.Documents.Find(id);

            if (document == null)
            {
                return NotFound();
            }

            byte[] fileBytes = document.FileData;

            if (fileBytes == null)
            {
                return StatusCode(404, "File not found");
            }

            string fileName = document.File;
            string fileType = document.FileType ?? "application/octet-stream";

            return File(fileBytes, fileType, fileName);
        }


        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult ViewFile(int id)
        {
            // Fetch the document from the database
            var document = dcms.Documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
            {
                // If document doesn't exist, redirect to Index
                return RedirectToAction("Index", "Document");
            }

            // Build the full file path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents", document.File);

            // Determine the file extension and mimeType early on
            string fileExtension = Path.GetExtension(document.File).ToLower();
            string mimeType = GetMimeType(fileExtension);  // Assuming GetMimeType is a method you have implemented

            // Check if the file can be viewed through Google Docs Viewer
            if (IsGoogleDocsSupported(fileExtension))
            {
                // Construct the Google Docs Viewer URL
                string fileUrl = "https://docs.google.com/viewer?url=" + Uri.EscapeDataString("https://yourdomain.com/uploads/documents/" + document.File);
                return Redirect(fileUrl);  // Redirect to Google Docs Viewer
            }

            try
            {
                // Read the file bytes
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, mimeType);  // Return file with the correct mime type
            }
            catch (Exception ex)
            {
                // Log the exception (use logging framework)
                // For now, we simply redirect back to Index
                return RedirectToAction("Index", "Document");
            }
        }


        // Phương thức để xác định MIME type cho tệp
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]

        private string GetMimeType(string extension)
        {
            switch (extension)
            {
                case ".pdf": return "application/pdf";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".jpg": return "image/jpg";
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".bmp": return "image/bmp";
                case ".svg": return "image/svg+xml";
                case ".webp": return "image/webp";
                case ".txt": return "text/plain";
                case ".xml": return "application/xml";
                case ".csv": return "text/csv";
                case ".json": return "application/json";
                case ".md": return "text/markdown";
                default: return "application/octet-stream"; // Default binary file type
            }
        }

        // Phương thức kiểm tra các định dạng file có thể mở bằng Google Docs Viewer
        private bool IsGoogleDocsSupported(string extension)
        {
            var supportedExtensions = new[] { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };
            return supportedExtensions.Contains(extension);
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult GetAllDocument(int pageNumber = 1)
        {
            var pageSize = 6;
            var pagedDcms = dcms.Documents.OrderBy(i => i.Id).ToPagedList(pageNumber, pageSize);
            var recentDcms = dcms.Documents.OrderByDescending(i => i.CreateTime).Take(4).ToList();

            ViewBag.PagedDcms = pagedDcms;
            ViewBag.RecentDcms = recentDcms;

            return View();
        }

    }
}
