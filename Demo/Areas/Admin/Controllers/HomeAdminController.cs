using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Demo.Areas.Admin.Controllers
{   
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("DangNhap"); ;
            }
            else
            {
                return View();
            }
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string user, string password)
        {
            //check db
            //check code
            if (user.ToLower() == "admin" && password == "123456")
            {
                Session["user"] = "admin";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Tài Khoản Đăng Nhập Không Đúng!";
                return View();
            }

        }

        public ActionResult DangXuat()
        {
            //Xóa session
            Session.Remove("user");
            //xóa session liên quan đếm form
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap");
        }
    }
}