using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class CartItem
    {
        public Ring _ring { get; set; }  // Sản phẩm kiểu Ring
        public int _quantity { get; set; }  // Số lượng sản phẩm trong giỏ hàng
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();  // Danh sách các mục trong giỏ hàng

        public IEnumerable<CartItem> Items
        { get { return items; } }  // Thuộc tính để truy cập danh sách các mục trong giỏ hàng

        // Phương thức để thêm sản phẩm kiểu Ring vào giỏ hàng
        public void Add_Ring_Cart(Ring _ring, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._ring.RingID == _ring.RingID);
            if (item == null)
                items.Add(new CartItem
                {
                    _ring = _ring,
                    _quantity = _quan
                });
            else item._quantity += _quan;
        }

        // Phương thức để tính tổng số lượng sản phẩm trong giỏ hàng
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }

        // Phương thức để tính tổng tiền của giỏ hàng
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * s._ring.Price);
            return (decimal)total;
        }

        // Phương thức để cập nhật số lượng sản phẩm trong giỏ hàng
        public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s => s._ring.RingID == id);
            if (item != null)
                item._quantity = _new_quan;
        }

        // Phương thức để xóa một mục (sản phẩm) khỏi giỏ hàng
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._ring.RingID == id);
        }

        // Phương thức để xóa toàn bộ giỏ hàng
        public void ClearCart()
        {
            items.Clear();
        }
    }
}