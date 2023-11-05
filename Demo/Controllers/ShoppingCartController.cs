using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Models;  // Import model Ring từ namespace Demo.Models

namespace NIKE.Controllers
{
    public class ShoppingCartController : Controller
    {
        DPSportStoreEntities database = new DPSportStoreEntities();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        // Hiển thị giỏ hàng
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }

        // Action để tạo mới giỏ hàng
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Action để thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            var _ring = database.Rings.SingleOrDefault(s => s.RingID == id);
            if (_ring != null)
            {
                GetCart().Add_Ring_Cart(_ring);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        // Action để cập nhật số lượng sản phẩm trong giỏ hàng
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_ring = int.Parse(form["idRing"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.Update_quantity(id_ring, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        // Hiển thị số lượng sản phẩm trong giỏ hàng (sử dụng PartialView)
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }

        // Action để xử lý thanh toán
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = form["AddressDeliverry"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail();
                    _order_detail.IDOrder = _order.ID;
                    _order_detail.IDProduct = item._ring.RingID;
                    _order_detail.UnitPrice = (double)item._ring.Price;
                    _order_detail.Quantity = item._quantity;
                    database.OrderDetails.Add(_order_detail);
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");
            }
            catch
            {
                return Content("Error checkout. Please check customer information... Thanks.");
            }
        }

        // Hiển thị trang thanh toán thành công
        public ActionResult CheckOut_Success()
        {
            return View();
        }
    }
}
