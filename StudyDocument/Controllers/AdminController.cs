using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using System.Security.Claims;
using X.PagedList.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudyDocument.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminRole")]
    public class AdminController : Controller
    {
        //private readonly StudyPlatformBkapContext admins = new StudyPlatformBkapContext();

        private readonly StudyPlatform_BkapContext admins;
        public AdminController(StudyPlatform_BkapContext _admins)
        {
            this.admins = _admins;
        }
        // GET: AdminController   
        public IActionResult Index(int pageNumber =1)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = admins.Admins.OrderBy(a => a.Id).ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }

        // GET: AdminController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        public ActionResult Create(Admin data)
        {
            data.Role = 1;
            data.IdCard = "";
            data.DateOfIssue = DateTime.Now;
            data.PlaceOfIssue = "";
            data.CreateTime = DateTime.Now;
            data.Password = Cipher.GenerateMD5(data.Password);


            var file = Request.Form.Files.FirstOrDefault();

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

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
                data.Avatar = fileName;
            }
            admins.Admins.Add(data);
            admins.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AdminController/Edit/5
        public IActionResult Edit(int id)
        {
            var admin = admins.Admins.Find(id);
            return View(admin);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        public ActionResult Edit(Admin data)
        {
            data.Role = 1;
            data.IdCard = "";
            data.DateOfIssue = DateTime.Now;
            data.PlaceOfIssue = "";
            data.CreateTime = DateTime.Now;
            data.Password = Cipher.GenerateMD5(data.Password);


            var file = Request.Form.Files.FirstOrDefault();

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

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
                data.Avatar = fileName;
            }
            admins.Admins.Update(data);
            admins.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AdminController/Delete/5
        public IActionResult Delete(int id)
        {
            var admin = admins.Admins.Find(id);
            if(admin == null)
            {
                return NotFound();
            }
            admins.Admins.Remove(admin);
            admins.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Admin data)
        {
            if (string.IsNullOrEmpty(data.UserName) || string.IsNullOrEmpty(data.Password))
            {
                ViewBag.Error = "<div class='alert alert-danger'>Tên đăng nhập và mật khẩu không được để trống</div>";
                return View(data);
            }

            var passmd5 = Cipher.GenerateMD5(data.Password);
            var account = admins.Admins.SingleOrDefault(x =>
                x.UserName == data.UserName && x.Password == passmd5);

            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim("id", account.Id.ToString()),
                    new Claim("fullname", account.FullName ?? ""),
                    new Claim("avatar",account.Avatar),
                    new Claim(ClaimTypes.Role, "AdminRole")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                var login = HttpContext.SignInAsync("AdminRole", principal, authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "<div class='alert alert-danger'>Tên đăng nhập hoặc mật khẩu sai</div>";
            return View(data);
        }


        [Authorize(Roles = "AdminRole")]
        public async Task<IActionResult> Logout()
        {
            var login = HttpContext.SignOutAsync("AdminRole");
            return RedirectToAction("Login", "Admin");
        }

    }
}
