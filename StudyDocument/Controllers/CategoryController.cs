using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;
using System.Text.RegularExpressions;
using X.PagedList.Extensions;

namespace StudyDocument.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
    public class CategoryController : Controller
    {

        private readonly StudyPlatform_BkapContext cats = new StudyPlatform_BkapContext();
        //static string Level = "";
        // GET: CategoryController
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Index(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var pageSize = 10;
            var pageList = cats.Categories.OrderBy(c => c.Id).ToPagedList(pageNumber, pageSize);
            return View(pageList);
        }

        // GET: CategoryController/Details/5
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Details(int id)
        {

            var cat = cats.Categories.Find(id);
            if( cat == null)
            {
                return NotFound($"Can not fint the category with id: {id}");
            }
            return View(cat);
        }

        // GET: CategoryController/Create
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        public IActionResult Create(Category data)
        {
            data.Level = "";
            data.Lang = "";
            data.Index = 0;
            data.Tag = "";
            data.Type = 0;
            data.Ord = 0;
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                data.Image = fileName;
            }
            cats.Categories.Add(data);
            cats.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cat = cats.Categories.Find(id);
            if (cat == null)
            {
                return NotFound();

            }
            else
            {
                return View(cat); 
            }

        }

        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpPost]
        public IActionResult Edit(Category data)
        {
            data.Level = "";
            data.Lang = "";
            data.Index = 0;
            data.Tag = "";
            data.Type = 0;
            data.Ord = 0;
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                data.Image = fileName;
            }
            else
            {
                // Lấy lại avatar cũ nếu không upload mới
                var oldData = cats.Categories.AsNoTracking().FirstOrDefault(u => u.Id == data.Id);
                if (oldData != null)
                {
                    data.Image = oldData.Image;
                }
            }
            cats.Categories.Update(data);
            cats.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CategoryController/Delete/5
        [Authorize(AuthenticationSchemes = "AdminRole,UserRole")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cat = cats.Categories.Find(id);
            if(cat == null)
            {
                return NotFound($"Can not fint the category with id: {id}");
            }
            cats.Categories.Remove(cat);
            cats.SaveChanges();
            return RedirectToAction("Index");
        }


        #region Name To Tag
        public static string NameToTag(string strName)
        {
            string strReturn = strName.Trim().ToLower();
            //strReturn = GetContent(strReturn, 150);
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            strReturn = Regex.Replace(strReturn, "[^\\w\\s]", string.Empty);
            string strFormD = strReturn.Normalize(System.Text.NormalizationForm.FormD);
            strReturn = regex.Replace(strFormD, string.Empty).Replace("đ", "d");
            strReturn = Regex.Replace(strReturn, "(-+)", " ");
            strReturn = Regex.Replace(strReturn.Trim(), "( +)", "-");
            strReturn = Regex.Replace(strReturn.Trim(), "(?+)", "");
            return strReturn;
        }
        #endregion
    }
}
