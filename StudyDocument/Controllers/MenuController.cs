using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;

using System.Linq;
using System.Text.RegularExpressions;
using X.PagedList;
using X.PagedList.Extensions;

namespace Project_web.Controllers
{
    public class MenuController : Controller
    {
        private readonly StudyPlatform_BkapContext pages = new StudyPlatform_BkapContext();
        static string Level = "";

        // GET: Page
        // Lấy danh sách các trang với phân trang
        public ActionResult Index(int pageNumber = 1)
        {
            int pagesize = 10;  // Số lượng trang hiển thị trên mỗi trang
            var pageList = pages.Menus.OrderBy(x => x.Id).ToPagedList(pageNumber, pagesize);  // Lấy danh sách trang và phân trang
            return View(pageList);  // Trả về view với danh sách các trang đã phân trang
        }

        // GET: Page/Create
        // Hiển thị form tạo mới trang
        public ActionResult Create(string? strLevel)
        {

            if (strLevel != null)
                Level = strLevel;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Menu data)
        {
            if (data.Status == null)
            {
                data.Status = true;
            }
            data.Tag = NameToTag(data.Name);
            if (data.Name != null)
            {


                data.Content = "";
                data.Type = 0;
                data.Index = 0;
                data.Lang = "";
                data.Link = "";
                data.Detail = "";
                data.Level = Level + data.Level;
                data.Level = Level + "00000";
                Level = "";


                pages.Menus.Add(data);
                pages.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                List<Menu> page = pages.Menus.ToList();
                ViewBag.Pages = page;
                return View(data);
            }
        }

        // GET: Page/Edit/5
        // Hiển thị form chỉnh sửa thông tin trang theo id
        public ActionResult Edit(int id)
        {
            Menu page = pages.Menus.First(p => p.Id == id);
            if (page == null)
            {
                return NotFound();
            }
            Level = page.Level.Substring(0, page.Level.Length - 5);
            return View(page);
        }

        // POST: Page/Edit/5
        // Xử lý khi người dùng gửi form chỉnh sửa thông tin trang
        [HttpPost]
        public ActionResult Edit(Menu data)
        {

            if (data.Status == null)
            {
                data.Status = true;
            }

            data.Tag = NameToTag(data.Name);
            data.Content = "";
            data.Type = 0;
            data.Index = 0;
            data.Lang = "";
            data.Link = "";
            data.Detail = "";
            data.Level = Level + data.Level;
            data.Level = Level + "00000";
            Level = "";


            pages.Menus.Update(data);
            pages.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Page/Delete/5
        // Xử lý xóa trang theo id
        public ActionResult Delete(int id)
        {
            Menu page = pages.Menus.First(p => p.Id == id);  // Tìm trang theo id
            if (page != null)
            {
                pages.Menus.Remove(page);  // Xóa trang khỏi cơ sở dữ liệu
                pages.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu
            }
            return RedirectToAction("Index");  // Chuyển hướng về trang danh sách trang
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
