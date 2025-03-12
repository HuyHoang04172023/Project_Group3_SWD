using A_LIÊM_SHOP.Extensions;
using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Drawing.Drawing2D;
using X.PagedList.Extensions;

namespace A_LIÊM_SHOP.Areas.Saler.Controllers
{
    [Area("Saler")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var orders = _orderService.GetAll();
            return View(orders);
        }
        public IActionResult Detail(int id = 0)
        {
            var order = _orderService.GetById(id);
            return View(order);
        }
        public IActionResult UpdateStatus(int id = 0, int status = 1)
        {
            var order = _orderService.GetById(id);
            if(status == 0)
            {
                //cancel
                order.OrderStatus = "Canceled";
            }
            else
            {
                //complete
                order.OrderStatus = "Completed";
            }
            order.EndDate = DateTime.Now;
            _orderService.UpdateOrder(order);
            return RedirectToAction("Index");
        }

    }
}
