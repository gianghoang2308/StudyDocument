using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MyDocument = StudyDocument.Models.Document;
using Microsoft.AspNetCore.Authorization;

namespace StudyDocument.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IConfiguration _configuration;
        //private static Admin? temp;
        private StudyPlatform_BkapContext _index = new StudyPlatform_BkapContext();
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            //_configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CreateBlog()
        {
            return View();
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult CreateAdmin(Admin data)
        //{
        //    temp = data;
        //    return RedirectToAction(nameof(Details));
        //}
      
        //public IActionResult Details()
        //{
        //    return View(temp);
        //}
        [AllowAnonymous]
        public IActionResult Index()
        {
            //Image
            List<Image> images = _index.Images.OrderByDescending(i => i.CreateTime).Take(3).ToList();

            ViewBag.ImageList = images;

            //Video
            List<StudyDocument.Models.Video> videos = _index.Videos.OrderByDescending(v =>  v.CreateTime).Take(3).ToList();
            ViewBag.VideoList = videos;

            //Document
            List<MyDocument> dcms = _index.Documents.OrderByDescending(v => v.CreateTime).Take(3).ToList();
            ViewBag.DocumentList = dcms;
            return View();

        }
        [AllowAnonymous]
        public IActionResult About()
        {
            List<Admin> admins = _index.Admins.OrderBy(a => a.Id).Take(6).ToList();
            ViewBag.AdminList = admins;

            List<Config> cf = _index.Configs.ToList();
            ViewBag.ConfigList = cf;
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            List<Contact> ct = _index.Contacts.Where(c => c.Id == 1).ToList();
            ViewBag.ContactList = ct;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Contact(Contact data)
        {
            data.Address = "";
            data.Status = true;
            data.CreateTime = DateTime.Now;
            _index.Contacts.Add(data);
            _index.SaveChanges();
            return RedirectToAction("Contact");
        }

        public IActionResult FAQ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[AllowAnonymous]
        //public IActionResult SendMail(IFormCollection form)
        //{
        //    string emailTo = form["email"].ToString();

        //    if (string.IsNullOrWhiteSpace(emailTo))
        //    {
        //        TempData["ErrorMessage"] = "Please enter a valid email address.";
        //        return RedirectToAction("Index");
        //    }

        //    StringBuilder mailBody = new StringBuilder();
        //    mailBody.Append($@"
        //        <html lang='vi'>
        //          <head>
        //            <meta charset='UTF-8'>
        //            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        //            <title>Thông Báo Đăng Ký Thành Công</title>
        //            <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
        //            <style>
        //              body {{
        //                font-family: Arial, sans-serif;
        //                background-color: #f8f9fa;
        //              }}
        //              .email-container {{
        //                background-color: #ffffff;
        //                max-width: 600px;
        //                margin: 30px auto;
        //                padding: 20px;
        //                border-radius: 8px;
        //                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        //              }}
        //              .email-header {{
        //                color: #28a745;
        //              }}
        //              .email-footer {{
        //                font-size: 14px;
        //                color: #6c757d;
        //                margin-top: 20px;
        //              }}
        //              .btn-custom {{
        //                background-color: #28a745;
        //                color: white;
        //                padding: 10px 20px;
        //                text-decoration: none;
        //                border-radius: 5px;
        //                display: inline-block;
        //                margin-top: 20px;
        //              }}
        //              .btn-custom:hover {{
        //                background-color: #218838;
        //              }}
        //            </style>
        //          </head>
        //          <body>
        //            <div class='container email-container'>
        //              <h2 class='email-header'>Đăng Ký Theo Dõi Thành Công!</h2>
        //              <p>Chào bạn, đây là hòm thư tự động</p>
        //              <p>Chúng tôi rất vui khi thông báo rằng bạn đã đăng ký thành công để nhận thông báo từ chúng tôi!</p>
        //              <p>Chúng tôi sẽ gửi đến bạn các thông báo và cập nhật mới nhất. Hãy chú ý theo dõi email của bạn!</p>
        //              <p>Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi.</p>
        //              <a href='#' class='btn-custom'>Truy cập website của chúng tôi</a>
        //              <div class='email-footer'>
        //                <p>Trân trọng,</p>
        //                <p>Đội ngũ Study Platform</p>
        //              </div>
        //            </div>
        //          </body>
        //        </html>");


        //    bool emailStatus = SendEmail("", emailTo, "Thông báo!!! Đây là hòm thư trả lời tự động", mailBody.ToString());

        //    if (emailStatus)
        //    {
        //        TempData["SuccessMessage"] = "Email sent successfully.";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Failed to send email.";
        //    }

        //    return RedirectToAction("Index");
        //}

        //[AllowAnonymous]
        //public bool SendEmail(string mailFrom, string toMail, string sub, string body)
        //{
        //    using (MailMessage mail = new MailMessage())
        //    {
        //        string Dislayname = _configuration["AppSettings:DisplayName"];
        //        mailFrom = _configuration["AppSettings:smtpUser"];

        //        mail.To.Add(toMail.Trim());
        //        mail.From = new MailAddress(mailFrom, Dislayname);
        //        mail.Subject = sub.Trim();
        //        mail.Body = body.Trim();
        //        mail.IsBodyHtml = true;

        //        using (SmtpClient smtp = new SmtpClient())
        //        {
        //            smtp.Host = _configuration["AppSettings:smtpServer"];
        //            smtp.Port = Convert.ToInt32(_configuration["AppSettings:smtpPort"]);
        //            smtp.EnableSsl = Convert.ToBoolean(_configuration["AppSettings:EnableSsl"]);
        //            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = new NetworkCredential(Convert.ToString(_configuration["AppSettings:smtpUser"]), Convert.ToString(_configuration["AppSettings:PwD"]));
        //            smtp.Timeout = 20000;
        //            smtp.Send(mail);
        //            return true;
        //        }
   
        //    }


        //}
    }
}
