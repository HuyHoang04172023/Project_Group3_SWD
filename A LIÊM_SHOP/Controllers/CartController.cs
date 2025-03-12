using A_LIÊM_SHOP.Extensions;
using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Proxy;
using A_LIÊM_SHOP.Repositories;
using A_LIÊM_SHOP.Services;
using A_LIÊM_SHOP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A_LIÊM_SHOP.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly GHNService _ghnService;
        public CartController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
            _ghnService = new GHNService();
        }
        public IActionResult Index()
        {
            var shoppingCart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart") ?? new List<Item>();
            return View(shoppingCart);
        }

        public IActionResult Add(int id = 0)
        {
            var p = _productService.GetProductById(id);
            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
            if (cart == null) //chua có sp trong giỏ
            {
                cart = new List<Item>();  ///tao new list
				cart.Add(new Item { Product = p, Quantity = 1 }); //them sp chưa có vào giỏ
            }
            else //có sp trong giỏ
            {
                Item existingItem = cart.FirstOrDefault(i => i.Product.Id == id);
                if (existingItem != null) // Nếu sản phẩm id{?} đã có trong giỏ hàng
                {
                    if (existingItem.Quantity < existingItem.Product.Quantity)
                    {
                        existingItem.Quantity += 1; // Tăng số lượng sản phẩm lên
                    }
                }
                else
                {
                    cart.Add(new Item { Product = p, Quantity = 1 }); //them sp thuôc id ? chưa có vào giỏ
                }
            }
            HttpContext.Session.SetObjectAsSession("cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult Sub(int id = 0)
        {
            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
            Item existingItem = cart.FirstOrDefault(i => i.Product.Id == id);
            if (existingItem.Quantity > 1)
            {
                existingItem.Quantity -= 1; // Giam số lượng sản phẩm lên
            }
            else
            {
                cart.Remove(existingItem);
            }
            HttpContext.Session.SetObjectAsSession("cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int id = 0)
        {
            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
            Item existingItem = cart.FirstOrDefault(i => i.Product.Id == id);
            cart.Remove(existingItem);
            HttpContext.Session.SetObjectAsSession("cart", cart);
            return RedirectToAction("Index");
        }

        

        [HttpGet]
		public IActionResult Checkout()
		{
			var shoppingCart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart") ?? new List<Item>();
            // Calculate the total price of items in the cart
            var total = shoppingCart.Sum(x => x.Quantity * x.Product.Price);

            // Check if the total is 0
            if (total == 0)
            {
                // If total is 0, redirect to Cart page
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.Cart = shoppingCart;
            return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(OrderViewModel orderView)
        {
            if (ModelState.IsValid)
            {
                User u = HttpContext.Session.GetObjectFromSession<User>("user");
                if (u != null)
                {
                    Order order = new Order();
                    order.UserId = u.Id;
                    List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
                    order.TotalAmountBefore = Math.Round((decimal)cart.Sum(x => x.Quantity * x.Product.Price), 2);
                    order.OrderDate = DateTime.Now;
                    order.PaymentMethod = "COD";
                    order.OrderStatus = "Processing";
                    order.Name = orderView.Name;
                    order.Address = orderView.Address;
                    order.Phone = orderView.Phone;

                    _orderService.AddOrder(order, cart);
                    _productService.reduceQuantity(cart);

                    HttpContext.Session.Remove("cart");

                    TempData["SuccessMessage"] = "Order successful!";
                    return RedirectToAction("Checkout");
                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Order failed! Please check your information.";
                return RedirectToAction("Checkout");
            }
        }
    }
}
