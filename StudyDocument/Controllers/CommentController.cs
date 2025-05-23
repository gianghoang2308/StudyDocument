using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;
using System.Drawing.Text;
using X.PagedList.Extensions;

namespace StudyDocument.Controllers
{
    [Authorize(Roles = "AdminRole")]
    public class CommentController : Controller
    {
        private readonly StudyPlatform_BkapContext cmts = new StudyPlatform_BkapContext();

        
        [HttpGet]
        public IActionResult Index(string imageName, string documentName, string videoName,
                            string newsName, string userName, bool? active, DateTime? timeRange,
                            int pageNumber = 1)
        {
            List<Image> img_id = cmts.Images.ToList(); ;
            ViewBag.ImageList = img_id;
            List<Video> video_id = cmts.Videos.ToList();
            ViewBag.VideoList = video_id;
            List<Document> document_id = cmts.Documents.ToList();
            ViewBag.DocumentList = document_id;
            List<User> users_id = cmts.Users.ToList();
            ViewBag.UserList = users_id;

            var query = cmts.Comments.AsQueryable();

            if (!string.IsNullOrEmpty(imageName))
            {
                query = query.Where(c => c.Image.Name.Contains(imageName)); 
            }

            if (!string.IsNullOrEmpty(documentName))
            {
                query = query.Where(c => c.Document.Title.Contains(documentName)); 
            }

            if (!string.IsNullOrEmpty(videoName))
            {
                query = query.Where(c => c.Video.Name.Contains(videoName)); 
            }

            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(c => c.User.FullName.Contains(userName)); 
            }

            if (active.HasValue)
            {
                query = query.Where(c => c.Status == active.Value); 
            }

            if (timeRange.HasValue)
            {
                query = query.Where(c => c.Date >= timeRange.Value);
            }


            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = query.Include(c => c.Image)    
                         .Include(c => c.Video)
                         .Include(c => c.Document)
                         .Include(c => c.User)
                         .OrderBy(c => c.Id)              
                         .ToPagedList(pageNumber, pageSize); 
            return View(pageList);
        }


        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var cmt = cmts.Comments.Find(id);
            if(cmt == null)
            {
                return NotFound($"Can not fint the comment with id: {id}");
            }
            return View(cmt);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Create()
        {
            List<Image> img = cmts.Images.ToList(); ;
            ViewBag.ImageList = img;
            List<Video> video = cmts.Videos.ToList();
            ViewBag.VideoList = video;
            List<Document> document = cmts.Documents.ToList();
            ViewBag.DocumentList = document;
            List<User> users = cmts.Users.ToList();
            ViewBag.UserList = users;

            return View();
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Create(Comment data)
        {
            List<Image> img = cmts.Images.ToList(); ;
            ViewBag.ImageList = img;
            List<Video> video = cmts.Videos.ToList();
            ViewBag.VideoList = video;
            List<Document> document = cmts.Documents.ToList();
            ViewBag.DocumentList = document;
            List<User> users = cmts.Users.ToList();
            ViewBag.UserList = users;


            if ((data.ImageId == null && data.VideoId == null && data.DocumentId == null) ||
            (new[] { data.ImageId, data.VideoId, data.DocumentId }.Count(id => id != null) > 1))

            {
                ModelState.AddModelError("", "Vui lòng chọn chính xác một loại khóa ngoại (Image, Video, Document).");
                return View(data);
            }

            data.Name = "";
            data.Address = "";
            data.Phone = "";
            data.Email = "";
            data.DateOfBirth = null;
            data.Date = DateTime.Now;
            data.Status = true;

            
            cmts.Comments.Add(data);
            cmts.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit Comment
        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
          
            var comment = cmts.Comments.FirstOrDefault(c => c.Id == id);


            if (comment == null)
            {
                return RedirectToAction("Index");
            }

            List<Image> img = cmts.Images.ToList();
            ViewBag.ImageList = img;
            List<Video> video = cmts.Videos.ToList();
            ViewBag.VideoList = video;
            List<Document> document = cmts.Documents.ToList();
            ViewBag.DocumentList = document;
            List<User> users = cmts.Users.ToList();
            ViewBag.UserList = users;

          
            return View(comment);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Edit(Comment data)
        {
            data.Name = "";
            data.Address = "";
            data.Phone = "";
            data.Email = "";
            data.DateOfBirth = null;
            data.Date = DateTime.Now;
            data.Status = true;

            
            if ((data.ImageId == null && data.VideoId == null && data.DocumentId == null) ||
                (new[] { data.ImageId, data.VideoId, data.DocumentId }.Count(id => id != null) > 1))
            {
                ModelState.AddModelError("", "Vui lòng chọn chính xác một loại khóa ngoại (Image, Video, Document).");
            }

           
            ViewBag.ImageList = cmts.Images.ToList();
            ViewBag.VideoList = cmts.Videos.ToList();
            ViewBag.DocumentList = cmts.Documents.ToList();
            ViewBag.UserList = cmts.Users.ToList();
          

         
            var comment = cmts.Comments.FirstOrDefault(c => c.Id == data.Id);
            if (comment == null)
            {
                return RedirectToAction("Index");
            }

           
            bool isUpdated = false;

            if (comment.ImageId != data.ImageId)
            {
                comment.ImageId = data.ImageId;
                isUpdated = true;
            }

            if (comment.VideoId != data.VideoId)
            {
                comment.VideoId = data.VideoId;
                isUpdated = true;
            }

            if (comment.DocumentId != data.DocumentId)
            {
                comment.DocumentId = data.DocumentId;
                isUpdated = true;
            }

            if (comment.UserId != data.UserId && data.UserId != 0)
            {
                comment.UserId = data.UserId;
                isUpdated = true;
            }

            if (comment.Message != data.Message)
            {
                comment.Message = data.Message;
                isUpdated = true;
            }

            if (isUpdated)
            {
                comment.Date = DateTime.Now;
                cmts.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cmt = cmts.Comments.Find(id);
            if(cmt == null)
            {
                return NotFound($"Can not find the comment with id: {id}");
            }
            cmts.Remove(cmt);
            cmts.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
