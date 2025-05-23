using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StudyDocument.Models;
using System.Reflection;
using X.PagedList.Extensions;

namespace StudyDocument.Controllers
{
    [Authorize(Roles = "AdminRole")]
    public class ConfigController : Controller
    {
        private readonly StudyPlatform_BkapContext cfs = new StudyPlatform_BkapContext();
        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            if(pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = cfs.Configs.OrderBy(cf => cf.Id).ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var cf = cfs.Configs.Find(id);
            if(cf == null)
            {
                return NotFound();
            }
            else
            {
                return View(cf);
            }
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Create(Config data)
        {
            data.MailPort = ""; /*Địa chỉ*/
            data.MailInfo = ""; /*Số điện thoại*/
            data.MailNoreply = "";
            data.MailPassword = "";
            data.PlaceHead = "";
            data.PlaceBody = "";
            data.LinkVideo1 = "";
            data.LinkVideo2 = "";
            data.LinkVideo3 = "";
            data.LinkVideo4 = "";
            data.GoogleId = "";
            data.Lang = "";
            cfs.Configs.Add(data);
            cfs.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cf = cfs.Configs.Find(id);
            if (cf == null)
            {
                return NotFound();
            }
            else
            {

                cfs.Update(cf);
                cfs.SaveChanges();
                return View(cf);
            }
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Edit(Config data)
        {
            data.MailPort = "";
            data.MailInfo = "";
            data.MailNoreply = "";
            data.MailPassword = "";
            data.PlaceHead = "";
            data.PlaceBody = "";
            data.LinkVideo1 = "";
            data.LinkVideo2 = "";
            data.LinkVideo3 = "";
            data.LinkVideo4 = "";
            data.GoogleId = "";
            data.Lang = "";
            cfs.Configs.Update(data);
            cfs.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cf = cfs.Configs.Find(id);
            if( cf == null)
            {
                return NotFound();
            }
           else
           {
                cfs.Remove(cf);
                cfs.SaveChanges();
                return RedirectToAction("Index");
           }
        }
    }
}
