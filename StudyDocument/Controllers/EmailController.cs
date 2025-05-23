using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace StudyDocument.Controllers
{
    public class EmailController : Controller
    {
       
            private readonly IConfiguration _configuration;
            private readonly StudyPlatform_BkapContext _context;

            public EmailController(IConfiguration configuration, StudyPlatform_BkapContext context)
            {
                _configuration = configuration;
                _context = context;
            }

          

        [HttpPost]
        public IActionResult Subscribe(Email model)
        {
            if (string.IsNullOrEmpty(model.EmailAddress))
            {
                ModelState.AddModelError("Email", "Email is required");
                return RedirectToAction("Index", "Home"); 
            }

            HttpContext.Session.SetString("Email", model.EmailAddress);

            try
            {
               
                var email = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
                var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

             
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = "Subscription Confirmation",
                    Body = "Bạn đã đăng ký nhận email từ StudyPlatform",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(model.EmailAddress);

            
                var smtpClient = new SmtpClient("smtp.your-email-provider.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(email, password),
                    EnableSsl = true,
                };

               
                smtpClient.Send(mailMessage);

            
                TempData["SuccessMessage"] = "Đăng ký nhận email thành công!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
     
                ModelState.AddModelError("Email", "Có lỗi khi gửi email: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }


        [AllowAnonymous]
        public IActionResult SendMail(IFormCollection form)
        {
            string emailTo = form["email"].ToString();

            if (string.IsNullOrWhiteSpace(emailTo))
            {
                TempData["ErrorMessage"] = "Please enter a valid email address.";
                return RedirectToAction("Index");
            }

            StringBuilder mailBody = new StringBuilder();
            mailBody.Append($@"
                <html lang='vi'>
                  <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Thông Báo Đăng Ký Thành Công</title>
                    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css' rel='stylesheet'>
                    <style>
                      body {{
                        font-family: Arial, sans-serif;
                        background-color: #f8f9fa;
                      }}
                      .email-container {{
                        background-color: #ffffff;
                        max-width: 600px;
                        margin: 30px auto;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                      }}
                      .email-header {{
                        color: #28a745;
                      }}
                      .email-footer {{
                        font-size: 14px;
                        color: #6c757d;
                        margin-top: 20px;
                      }}
                      .btn-custom {{
                        background-color: #28a745;
                        color: white;
                        padding: 10px 20px;
                        text-decoration: none;
                        border-radius: 5px;
                        display: inline-block;
                        margin-top: 20px;
                      }}
                      .btn-custom:hover {{
                        background-color: #218838;
                      }}
                    </style>
                  </head>
                  <body>
                    <div class='container email-container'>
                      <h2 class='email-header'>Đăng Ký Theo Dõi Thành Công!</h2>
                      <p>Chào bạn, đây là hòm thư tự động</p>
                      <p>Chúng tôi rất vui khi thông báo rằng bạn đã đăng ký thành công để nhận thông báo từ chúng tôi!</p>
                      <p>Chúng tôi sẽ gửi đến bạn các thông báo và cập nhật mới nhất. Hãy chú ý theo dõi email của bạn!</p>
                      <p>Nếu bạn có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi.</p>
                      <a href='#' class='btn-custom'>Truy cập website của chúng tôi</a>
                      <div class='email-footer'>
                        <p>Trân trọng,</p>
                        <p>Đội ngũ Study Platform</p>
                      </div>
                    </div>
                  </body>
                </html>");


            bool emailStatus = SendEmail("", emailTo, "Thông báo!!! Đây là hòm thư trả lời tự động", mailBody.ToString());

            if (emailStatus)
            {
                TempData["SuccessMessage"] = "Email sent successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to send email.";
            }

            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]
        public bool SendEmail(string mailFrom, string toMail, string sub, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                string Dislayname = _configuration["AppSettings:DisplayName"];
                mailFrom = _configuration["AppSettings:smtpUser"];

                mail.To.Add(toMail.Trim());
                mail.From = new MailAddress(mailFrom, Dislayname);
                mail.Subject = sub.Trim();
                mail.Body = body.Trim();
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = _configuration["AppSettings:smtpServer"];
                    smtp.Port = Convert.ToInt32(_configuration["AppSettings:smtpPort"]);
                    smtp.EnableSsl = Convert.ToBoolean(_configuration["AppSettings:EnableSsl"]);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(Convert.ToString(_configuration["AppSettings:smtpUser"]), Convert.ToString(_configuration["AppSettings:PwD"]));
                    smtp.Timeout = 20000;
                    smtp.Send(mail);
                    return true;
                }

            }


        }
    }
}
