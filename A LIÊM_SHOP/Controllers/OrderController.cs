using A_LIÊM_SHOP.Extensions;
using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Services;
using Microsoft.AspNetCore.Mvc;

namespace A_LIÊM_SHOP.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            User u = HttpContext.Session.GetObjectFromSession<User>("user");
            var orders = _orderService.GetByUserId(u.Id);
            return View(orders);
        }
        public IActionResult Detail(int id = 0)
        {
            var order = _orderService.GetById(id);
            return View(order);
        }
    }
}
