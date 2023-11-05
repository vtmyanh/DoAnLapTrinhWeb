using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Models;
using System.Net;
using Microsoft.Ajax.Utilities;
using System.Data.Entity;
using Newtonsoft.Json.Converters;

namespace Demo.Controllers
{
    public class CategoriesController : Controller
    {
        private DPSportStoreEntities database = new DPSportStoreEntities();

        // GET: Categories
        public ActionResult Index()
        {
            // Lấy danh sách các Category từ cơ sở dữ liệu và trả về view
            var categories = database.Categories.ToList();
            return View(categories);
        }

        // GET: Categories/Create
        [HttpGet]
        public ActionResult Create()
        {
            // Hiển thị form để tạo mới Category
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Thêm Category mới vào cơ sở dữ liệu và chuyển hướng đến trang danh sách
                    database.Categories.Add(category);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi, hiển thị thông báo lỗi
                    ModelState.AddModelError("", "Lỗi tạo mới Category: " + ex.Message);
                }
            }

            // Trả về view tạo mới Category với thông tin và thông báo lỗi (nếu có)
            return View(category);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            var category = database.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            // Hiển thị thông tin chi tiết về Category
            return View(category);
        }

        // GET: Categories/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = database.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            // Hiển thị form chỉnh sửa Category
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin Category và chuyển hướng đến trang danh sách
                    database.Entry(category).State = EntityState.Modified;
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi, hiển thị thông báo lỗi
                    ModelState.AddModelError("", "Lỗi chỉnh sửa Category: " + ex.Message);
                }
            }

            // Trả về view chỉnh sửa Category với thông tin và thông báo lỗi (nếu có)
            return View(category);
        }

        // GET: Categories/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = database.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            // Hiển thị thông tin Category để xác nhận xóa
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Xóa Category khỏi cơ sở dữ liệu và chuyển hướng đến trang danh sách
                var category = database.Categories.FirstOrDefault(c => c.Id == id);
                database.Categories.Remove(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, hiển thị thông báo lỗi
                ModelState.AddModelError("", "Không xóa được do có liên quan đến bảng khác: " + ex.Message);
                var deletedCategory = new Category { Id = id };
                return View(deletedCategory);
            }
        }
    }
}
