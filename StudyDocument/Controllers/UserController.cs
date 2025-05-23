using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace StudyDocumentPlatform.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]

    public class UserController : Controller
    {
        private readonly StudyPlatform_BkapContext users = new StudyPlatform_BkapContext();

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]

        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = users.Users.ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]

        public IActionResult Detail(int id)
        {
            var user = users.Users.Find(id);
            if (user == null)
            {
                return NotFound($"Can not find the user with id: {id}");
            }
            return View(user);
        }

         [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            //create selectlist for issue places
            var issuePlaces = new List<string>
            {
                "Cục quản lý hành chính về TTXH",
                "Cục quản lý xuất nhập cảnh",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bạc Liêu", "Bắc Giang", "Bắc Kạn",
                "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận",
                "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên"
            };

            // Passing the list to the View using ViewBag
            ViewBag.IssuePlaces = new SelectList(issuePlaces);

            // Optionally, pass a model to the view
            var model = new User(); // Replace with your actual model
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(User data)
        {
            //check image file
            var files = HttpContext.Request.Form.Files;
            string[] imageFields = new string[] { "Avatar", "Image1", "Image2", "Image3" };

            if (files.Count > 0)
            {
                data.Status = true;
                data.CreateTime = DateTime.Now;
                data.Role = 0;
                data.Password = Cipher.GenerateMD5(data.Password);
                // Kiểm tra và tải các ảnh mới lên
                for (int i = 0; i < files.Count && i < imageFields.Length; i++)
                {
                    var file = files[i];
                    if (file.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "users");

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

                        // Gán tên ảnh vào các trường tương ứng
                        if (i == 0) data.Avatar = fileName;
                        else if (i == 1) data.Image1 = fileName;
                        else if (i == 2) data.Image2 = fileName;
                        else if (i == 3) data.Image3 = fileName;
                    }
                }
            }


            //create selectlist for issue places
            var issuePlaces = new List<string>
            {
                "Cục quản lý hành chính về TTXH",
                "Cục quản lý xuất nhập cảnh",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bạc Liêu", "Bắc Giang", "Bắc Kạn",
                "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận",
                "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên"
            };
            ViewBag.IssuePlaces = new SelectList(issuePlaces);

          
          

            users.Users.Add(data);
            users.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = users.Users.Find(id);
            if (user == null)
            {
                return NotFound($"Can not find the user with id: {id}");

            }
            else
            {

                //create selectlist for issue places
                var issuePlaces = new List<string>
            {
                "Cục quản lý hành chính về TTXH",
                "Cục quản lý xuất nhập cảnh",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bạc Liêu", "Bắc Giang", "Bắc Kạn",
                "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận",
                "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên"
            };

                // Passing the list to the View using ViewBag
                ViewBag.IssuePlaces = new SelectList(issuePlaces);
                return View(user);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Edit(User data, int id)
        {
            // Binding data create time
            data.Status = true;
            data.CreateTime = DateTime.Now;
            data.Role = 0;
            data.Password = Cipher.GenerateMD5(data.Password);
            var issuePlaces = new List<string>
            {
                "Cục quản lý hành chính về TTXH",
                "Cục quản lý xuất nhập cảnh",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bạc Liêu", "Bắc Giang", "Bắc Kạn",
                "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận",
                "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên"
            };
            ViewBag.IssuePlaces = new SelectList(issuePlaces);

            
            // Lấy dữ liệu cũ của người dùng từ cơ sở dữ liệu
            var oldData = users.Users.AsNoTracking().FirstOrDefault(c => c.Id == data.Id);

            // Kiểm tra nếu có ảnh mới được tải lên
            var files = HttpContext.Request.Form.Files;
            string[] imageFields = new string[] { "Avatar", "Image1", "Image2", "Image3" };

            if (files.Count > 0)
            {
                // Kiểm tra và tải các ảnh mới lên
                for (int i = 0; i < files.Count && i < imageFields.Length; i++)
                {
                    var file = files[i];
                    if (file.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "users");

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

                        // Gán tên ảnh vào các trường tương ứng
                        if (i == 0) data.Avatar = fileName;
                        else if (i == 1) data.Image1 = fileName;
                        else if (i == 2) data.Image2 = fileName;
                        else if (i == 3) data.Image3 = fileName;
                    }
                }
            }
            else
            {
                // Nếu không có ảnh mới, giữ lại ảnh cũ từ cơ sở dữ liệu
                if (oldData != null)
                {
                    data.Avatar = oldData.Avatar; // Giữ ảnh cũ
                    data.Image1 = oldData.Image1;
                    data.Image2 = oldData.Image2;
                    data.Image3 = oldData.Image3;
                }
            }

            // Cập nhật và lưu dữ liệu vào cơ sở dữ liệu
            users.Users.Update(data);
            users.SaveChanges();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = users.Users.Find(id);
            if (user == null)
            {
                return NotFound("Can not find the user with id: " + id);
            }

            else
            {
                users.Users.Remove(user);
                users.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User data)
        {
            if (string.IsNullOrEmpty(data?.UserName) || string.IsNullOrEmpty(data?.Password))
            {
                ViewBag.Error = "<div class='alert alert-danger'>Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu</div>";
                return View(data);
            }

            var passmd5 = Cipher.GenerateMD5(data.Password);

            var account = users.Users.SingleOrDefault(x =>
                x.UserName == data.UserName && x.Password == passmd5);

            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim("id", account.Id.ToString()),
                    new Claim("fullname", account.FullName ?? ""),
                    new Claim("avatar", account.Avatar ?? ""),
                    new Claim(ClaimTypes.Role, "UserRole")
                };

                var identity = new ClaimsIdentity(claims, "UserRole");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                HttpContext.SignInAsync("UserRole", principal, authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "<div class='alert alert-danger'>Tên đăng nhập hoặc mật khẩu sai</div>";
            return View(data);
        }


        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync("UserRole");
            return RedirectToAction("Login", "User");
        }

        // Hiển thị form đăng ký
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            var issuePlaces = new List<string>
            {
                "Cục quản lý hành chính về TTXH",
                "Cục quản lý xuất nhập cảnh",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bạc Liêu", "Bắc Giang", "Bắc Kạn",
                "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận",
                "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên"
            };
            ViewBag.IssuePlaces = new SelectList(issuePlaces);
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(User data)
        {
            var issuePlaces = new List<string>
            {
                "Cục quản lý hành chính về TTXH",
                "Cục quản lý xuất nhập cảnh",
                "An Giang", "Bà Rịa - Vũng Tàu", "Bạc Liêu", "Bắc Giang", "Bắc Kạn",
                "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Bình Thuận",
                "Cà Mau", "Cao Bằng", "Cần Thơ", "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên"
            };
            ViewBag.IssuePlaces = new SelectList(issuePlaces);

            var existingUser = users.Users.FirstOrDefault(u => u.UserName == data.UserName || u.Email == data.Email);
            if (existingUser != null)
            {
                return View("ExistingUser", "User");
            }


            if (string.IsNullOrEmpty(data.Password))
            {
                ModelState.AddModelError("Password", "Vui lòng nhập mật khẩu.");
                return View(data);
            }
            data.Avatar = "";
            data.Image1 = "";
            data.Image2 = "";
            data.Image3 = "";
            data.CreateTime = DateTime.Now;
            data.Status = true;
            data.Role = 0;
            data.Password = Cipher.GenerateMD5(data.Password);

            users.Users.Add(data);
            users.SaveChanges();
            return RedirectToAction("Login", "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            // Lấy user hiện tại từ cơ sở dữ liệu (ví dụ: theo HttpContext.User.Identity.Name)
            var username = User.Identity.Name;
            var user = users.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Kiểm tra mật khẩu hiện tại
            var currentPasswordHash = Cipher.GenerateMD5(currentPassword);
            if (user.Password != currentPasswordHash)
            {
                ModelState.AddModelError("currentPassword", "Mật khẩu hiện tại không đúng.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không khớp.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                ModelState.AddModelError("newPassword", "Mật khẩu mới phải ít nhất 6 ký tự.");
                return View();
            }

            // Cập nhật mật khẩu
            user.Password = Cipher.GenerateMD5(newPassword);
            users.SaveChanges();

            ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
            return View();
        }



    }
}
