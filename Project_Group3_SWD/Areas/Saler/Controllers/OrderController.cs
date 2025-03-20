using Project_Group3_SWD.Extensions;
using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Drawing.Drawing2D;
using X.PagedList.Extensions;
using Project_Group3_SWD.Proxy;

namespace Project_Group3_SWD.Areas.Saler.Controllers
{
	[Area("Saler")]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly GHNService _ghnService;
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
			_ghnService = new GHNService();
		}
		public async Task<IActionResult> Index()
		{
			var orders = await _ghnService.GetAllOrders();
			ViewBag.Orders = orders;
			return View("~/Areas/Saler/Views/Order/Index.cshtml");
		}

		public async Task<IActionResult> Detail([FromQuery] string orderCode)
		{
			var order = await _ghnService.GetOrderDetailByOrderCode(orderCode);
			ViewBag.Order = order;
			return View("~/Areas/Saler/Views/Order/Detail.cshtml");
		}
		public IActionResult UpdateStatus(int id = 0, int status = 1)
		{
			var order = _orderService.GetById(id);
			if (status == 0)
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
		[HttpPost]
		public async Task<IActionResult> UpdateNote(string orderCode, string note = " ")
		{
			await _ghnService.UpdateOrderNoteById(orderCode, note);
			ViewBag.Message = "Update note with order code " + orderCode + " successfully";
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> CancelOrder(string orderCode)
		{
			await _ghnService.CancelOrderById(orderCode);
			return RedirectToAction("Index");
		}

	}
}
