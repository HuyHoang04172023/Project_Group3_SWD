using Project_Group3_SWD.Extensions;
using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Microsoft.AspNetCore.Mvc;

namespace Project_Group3_SWD.Controllers
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
