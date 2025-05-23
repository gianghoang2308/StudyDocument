using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyDocument.Models;
using System;
using X.PagedList.Extensions;

namespace StudyDocument.Controllers
{
    [Authorize(Roles = "AdminRole")]
    public class ContactController : Controller
    {
        private readonly StudyPlatform_BkapContext cts = new StudyPlatform_BkapContext();
        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            if(pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = cts.Contacts.OrderBy(c => c.Id).ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var ct = cts.Contacts.Find(id);
            if (ct == null)
            {
                return NotFound($"Can not find the contact with id: {id}");
            }
            return View(ct);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Create(Contact data)
        {
            data.CreateTime = DateTime.Now;
            data.Status = true;
            cts.Contacts.Add(data);
            cts.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ct = cts.Contacts.Find(id);
            if(ct == null)
            {
                return NotFound($"Can not fint the contact with id: {id}");
            }
            return View(ct);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Edit(Contact data)
        {
            data.CreateTime = DateTime.Now;
            data.Status = true;
            cts.Contacts.Update(data);
            cts.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ct = cts.Contacts.Find(id);
            if( ct == null)
            {
                return NotFound($"Can not fint the contact with id: {id}");
            }
            cts.Contacts.Remove(ct);
            cts.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
