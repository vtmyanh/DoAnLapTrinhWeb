    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
using System.Web.Security;
using Demo.Models;

namespace Demo.Controllers
{
    public class LoginUserController : Controller
    {
        DPSportStoreEntities database = new DPSportStoreEntities();
        // GET: LoginUser
        //Customer
        public ActionResult RegisterCustomer() //thiết kế form Đăng ký:
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(customer.EmailCus) || string.IsNullOrEmpty(customer.PassCus) || string.IsNullOrEmpty(customer.PhoneCus))
                {
                    ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin đăng ký.");
                    return View(customer);
                }
                // Kiểm tra xem mật khẩu và mật khẩu xác nhận có khớp nhau không
                if (customer.PassCus != customer.ConfirmPassCus)
                {
                    ModelState.AddModelError("ConfirmPassCus", "Mật khẩu xác nhận không khớp với mật khẩu.");
                    return View(customer);
                }

                // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu chưa
                var existingEmail = database.Customers.FirstOrDefault(c => c.EmailCus == customer.EmailCus);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("EmailCus", "Email này đã được sử dụng.");
                }

                // Kiểm tra xem số điện thoại đã tồn tại trong cơ sở dữ liệu chưa
                var existingPhone = database.Customers.FirstOrDefault(c => c.PhoneCus == customer.PhoneCus);
                if (existingPhone != null)
                {
                    ModelState.AddModelError("PhoneCus", "Số điện thoại này đã được sử dụng.");
                }

                // Kiểm tra xem mật khẩu đáp ứng tiêu chuẩn (ít nhất một chữ cái viết hoa và một chữ số, và có ít nhất 9 ký tự)
                if (!Regex.IsMatch(customer.PassCus, @"^(?=.*[A-Z])(?=.*\d).{9,}$"))
                {
                    ModelState.AddModelError("PassCus", "Mật khẩu phải chứa ít nhất một chữ cái viết hoa và một chữ số, và có ít nhất 9 ký tự.");
                }

                if (ModelState.IsValid)
                {
                    // Thêm khách hàng mới vào cơ sở dữ liệu nếu không có lỗi
                    database.Customers.Add(customer);
                    database.SaveChanges();
                    // Đăng ký thành công, đặt thông báo vào ViewBag
                    ViewBag.SuccessMessage = "Đăng ký thành công. Bây giờ bạn có thể đăng nhập.";
                    // Chuyển hướng đến trang đăng nhập
                    return View("LoginCustomer");
                }
            }
            else
            {
                // Nếu có lỗi, hiển thị trang đăng ký lại với thông báo lỗi và dữ liệu đã nhập trước đó
                return View(customer);
            }
            return View(customer);
        }


        public ActionResult LoginCustomer() //thiết kế form Login:
        {
            return View();
        }
        //Trang đăng nhập
        [HttpPost]
        public ActionResult LoginCustomer(Customer cust)
        {
            if (string.IsNullOrEmpty(cust.EmailCus))
                ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
            if (string.IsNullOrEmpty(cust.PassCus))
                ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");

            if (ModelState.IsValid)
            {
                if (cust.EmailCus == "admin" && cust.PassCus == "123456")
                {
                    // Nếu là admin, chuyển hướng đến trang khác (ví dụ: AdminDashboard)
                    return RedirectToAction("DangNhap", "HomeAdmin", new { area = "Admin" });
                }

                var khachhang = database.Customers.FirstOrDefault(k => k.EmailCus == cust.EmailCus && k.PassCus == cust.PassCus);
                if (khachhang != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["taikhoan"] = khachhang;
                    return RedirectToAction("RegisterCustomer");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }


        //Hàm thoát người dùng
        public ActionResult LogOutUser()
        {
            // Hủy bỏ phiên làm việc của người dùng
            Session.Abandon();

            // Chuyển hướng về trang đăng nhập sau khi đăng xuất
            return RedirectToAction("LoginCustomer","LoginUser");
        }

       
    }
}