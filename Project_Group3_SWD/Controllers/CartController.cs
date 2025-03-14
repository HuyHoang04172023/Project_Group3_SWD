using System.Net;
using System.Security.Cryptography.Xml;
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
		public IActionResult Confirm()
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
        public IActionResult Confirm(OrderViewModel orderView)
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
        public int ConvertUsdToVnd(float codAmountUsd, float exchangeRate = 24000f)
        {
            return (int)Math.Round(codAmountUsd * exchangeRate);
        }


        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(string to_name, string to_phone, string to_address,
            string to_ward_code, int to_district_id, 
            float cod_amount,int service_id, string payment_method)
        {

            if(payment_method == "online-payment")
            {
                //Lam VnPay
            }

            if (payment_method == "bank-transfer")
            {
                //Lam PayOS
            }
            

            var shopsJson = await _ghnService.GetShopsAsync();
            var shops = shopsJson["data"]?["shops"]?.ToObject<List<ShopModel>>() ?? new List<ShopModel>();
            ViewBag.from_name = shops[0].Name;
            ViewBag.from_phone = shops[0].Phone;
            ViewBag.from_address = shops[0].Address;
            ViewBag.to_name = to_name;
            ViewBag.to_phone = to_phone;
            ViewBag.to_address = to_address;
            ViewBag.to_ward_code = to_ward_code;
            ViewBag.to_district_id = to_district_id;
            ViewBag.cod_amount = ConvertUsdToVnd(cod_amount);
            ViewBag.service_id = service_id;
            ViewBag.payment_method = payment_method;

            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");

            var items = cart.Select(item => new
            {
                name = item.Product.Name,
                quantity = item.Quantity,
                price = item.Product.Price,
                weight = 200
            }).ToArray();
            foreach (var item in items)
            {
                Console.WriteLine($"Tên: {item.name}, Số lượng: {item.quantity}, Giá: {item.price}");
            }
            ViewBag.items = items;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShippingOrder(string from_name, string from_phone, string from_address, string to_name, string to_phone, string to_address, string to_ward_code, int to_district_id, int cod_amount, int service_id)
        {
            var orderJson = await _ghnService.CreateShippingOrderAsync(from_name, from_phone,from_address,to_name,to_phone,to_address,to_ward_code,to_district_id,cod_amount,service_id);
            if (orderJson["code"]?.ToObject<int>() == 200)
            {
                return Ok(new { message = "Order created successfully", order_code = orderJson["data"]?["order_code"] });
            }
            return BadRequest(new { message = "Failed to create order", error = orderJson["message"] });
        }
    }
}
