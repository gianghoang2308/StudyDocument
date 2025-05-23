using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;

namespace StudyDocument.Controllers
{
    public class SearchController : Controller
    {
        private readonly StudyPlatform_BkapContext _context;

        public SearchController(StudyPlatform_BkapContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Nếu không có từ khóa, trả về View trống hoặc mặc định
                return View();
            }

            // Tìm trong bảng Image
            var images = _context.Images
                .Where(i => i.Name.Contains(searchTerm))
                .ToList();

            // Tìm trong bảng Video
            var videos = _context.Videos
                .Where(v => v.Title.Contains(searchTerm) || v.Description.Contains(searchTerm))
                .ToList();

            // Tìm trong bảng Document
            var documents = _context.Documents
                .Where(d => d.Title.Contains(searchTerm) || d.Description.Contains(searchTerm))
                .ToList();

            // Tạo một ViewModel chứa cả 3 kết quả
            var model = new SearchModel
            {
                ImageList = images,
                VideoList = videos,
                DocumentList = documents,
                SearchTerm = searchTerm
            };

            return View(model);
        }
    }
}

