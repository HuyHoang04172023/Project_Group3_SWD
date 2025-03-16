using System.Net;
using System.Security.Cryptography.Xml;
using A_LIÊM_SHOP.Extensions;
using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Proxy;
using A_LIÊM_SHOP.Repositories;
using A_LIÊM_SHOP.Services;
using A_LIÊM_SHOP.ViewModels;
using A_LIÊM_SHOP.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project_Group3_SWD.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly GHNService _ghnService;
        private readonly IVnPayService _vnPayService;


        public CartController(IProductService productService, IOrderService orderService, IVnPayService vnPayService)
        {
            _productService = productService;
            _orderService = orderService;
            _ghnService = new GHNService();
            _vnPayService = vnPayService;


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
                ShippingOrderModel order = new ShippingOrderModel();
                order.to_name = to_name;
                order.to_phone = to_phone;
                order.to_address = to_address;
                order.to_ward_code = to_ward_code;
                order.to_district_id = to_district_id;
                order.cod_amount = cod_amount;
                order.service_id = service_id;
                order.payment_method = payment_method;

                HttpContext.Session.SetObjectAsSession("order", order);
                return RedirectToAction("CheckoutWithVNPay", new { totalAmount = ConvertUsdToVnd(cod_amount), name = to_name });
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

            List<ItemViewModel> items = cart.Select(item => new ItemViewModel
            {
                Name = item.Product.Name,
                Quantity = item.Quantity,
                Price = (int)item.Product.Price,
                Weight = 200
            }).ToList();
            foreach (var item in items)
            {
                Console.WriteLine($"Tên: {item.Name}, Số lượng: {item.Quantity}, Giá: {item.Price}");
            }
            ViewBag.items = items;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShippingOrder(string from_name, string from_phone, string from_address, 
            string to_name, string to_phone, string to_address, string to_ward_code, 
            int to_district_id, int cod_amount, int service_id)
        {
            User u = HttpContext.Session.GetObjectFromSession<User>("user");
            if (u != null)
            {
                Order order = new Order();
                order.UserId = u.Id;
                List<Item> cartUser = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
                order.TotalAmountBefore = Math.Round((decimal)cartUser.Sum(x => x.Quantity * x.Product.Price), 2);
                order.OrderDate = DateTime.Now;
                order.PaymentMethod = "COD";
                order.OrderStatus = "Processing";
                order.Name = to_name;
                order.Address = to_address;
                order.Phone = to_phone;

                _orderService.AddOrder(order, cartUser);
                _productService.reduceQuantity(cartUser);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }

            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");

            List<ItemViewModel> items = cart.Select(item => new ItemViewModel
            {
                Name = item.Product.Name,
                Quantity = item.Quantity,
                Price = (int)item.Product.Price,
                Weight = 200
            }).ToList();

            var orderJson = await _ghnService.CreateShippingOrderAsync(from_name, from_phone,from_address,
                to_name,to_phone,to_address,to_ward_code,to_district_id,
                cod_amount,service_id, items);
            if (orderJson["code"]?.ToObject<int>() == 200)
            {
                HttpContext.Session.Remove("cart");

                TempData["SuccessMessage"] = "Order successful!";
                return Redirect("/");

            }
            return BadRequest(new { message = "Failed to create order", error = orderJson["message"] });
        }

        [HttpGet]
        public IActionResult CheckoutWithVNPay(int totalAmount, string name)
        {
            Console.WriteLine(ConvertUsdToVnd(totalAmount));
            Console.WriteLine(name);

            var paymentModel = new PaymentInformationModel
            {
                Amount = totalAmount,
                OrderDescription = $"Thanh toán đơn hàng",
                Name = name,
                OrderType = "billpayment"
            };

            // Gọi API VNPay để tạo URL thanh toán
            string vnpayUrl = _vnPayService.CreatePaymentUrl(paymentModel, HttpContext);

            return Redirect(vnpayUrl); // Chuyển hướng đến VNPay
        }
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query) as PaymentResponseModel;

            if (response != null && response.Success)
            {
                return RedirectToAction("PaymentSuccess", "Cart"); // Điều hướng đến Cart/PaymentSuccess
            }

            TempData["ErrorMessage"] = $"Giao dịch thất bại! Mã lỗi: {response?.VnPayResponseCode}";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult PaymentSuccess()
        {
            ShippingOrderModel order = HttpContext.Session.GetObjectFromSession<ShippingOrderModel>("order");

            if (order == null)
            {
                return RedirectToAction("Cart"); // Xử lý khi không có đơn hàng
            }

            // Lưu dữ liệu vào TempData để chuyển sang phương thức POST
            TempData["to_name"] = order.to_name;
            TempData["to_phone"] = order.to_phone;
            TempData["to_address"] = order.to_address;
            TempData["to_ward_code"] = order.to_ward_code;
            TempData["to_district_id"] = order.to_district_id;
            TempData["cod_amount"] = 0;
            TempData["service_id"] = order.service_id;
            TempData["payment_method"] = "cod";

            return View(); // Chuyển hướng đến một GET action
        }

    }
}
