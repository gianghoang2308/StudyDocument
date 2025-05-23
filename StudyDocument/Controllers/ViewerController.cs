using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using System;
using System.Linq;

namespace StudyDocumentPlatform.Controllers
{
    [Authorize(Roles = "AdminRole")]
    public class ViewerController : Controller
    {
        private readonly StudyPlatform_BkapContext _context;

        // Constructor to inject the context
        public ViewerController(StudyPlatform_BkapContext context)
        {
            _context = context;
        }

        // Action to get the total number of logins for the current week
        public IActionResult GetLoginCountByWeek()
        {
            var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek); // Start of the current week (Sunday)
            var endOfWeek = startOfWeek.AddDays(7);  // End of the current week (Saturday)

            var loginCountWeek = _context.Viewers
                                         .Where(v => v.AccessTime >= startOfWeek && v.AccessTime <= endOfWeek)
                                         .Count();

            ViewData["LoginCountWeek"] = loginCountWeek;
            return View(); // Return view to display the count
        }

        // Action to get the total number of logins for the current month
        public IActionResult GetLoginCountByMonth()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // First day of the current month
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // Last day of the current month

            var loginCountMonth = _context.Viewers
                                          .Where(v => v.AccessTime >= startOfMonth && v.AccessTime <= endOfMonth)
                                          .Count();

            ViewData["LoginCountMonth"] = loginCountMonth;
            return View(); // Return view to display the count
        }

        // Action to get the total number of logins for the current year
        public IActionResult GetLoginCountByYear()
        {
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1); // First day of the current year
            var endOfYear = new DateTime(DateTime.Now.Year, 12, 31); // Last day of the current year

            var loginCountYear = _context.Viewers
                                         .Where(v => v.AccessTime >= startOfYear && v.AccessTime <= endOfYear)
                                         .Count();

            ViewData["LoginCountYear"] = loginCountYear;
            return View(); // Return view to display the count
        }

        public IActionResult GetLoginStats()
        {
            var now = DateTime.Now;

            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var startOfYear = new DateTime(now.Year, 1, 1);
            var endOfYear = new DateTime(now.Year, 12, 31);

            ViewData["LoginCountWeek"] = _context.Viewers
                .Where(v => v.AccessTime >= startOfWeek && v.AccessTime <= endOfWeek)
                .Select(v => v.PublicIp)
                .Distinct()
                .Count();

            ViewData["LoginCountMonth"] = _context.Viewers
                .Where(v => v.AccessTime >= startOfMonth && v.AccessTime <= endOfMonth)
                .Select(v => v.PublicIp)
                .Distinct()
                .Count();

            ViewData["LoginCountYear"] = _context.Viewers
                .Where(v => v.AccessTime >= startOfYear && v.AccessTime <= endOfYear)
                .Select(v => v.PublicIp)
                .Distinct()
                .Count();

            return View();
        }

    }
}
