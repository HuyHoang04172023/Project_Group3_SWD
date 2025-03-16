using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Project_Group3_SWD.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Project_Group3_SWD.Extensions;


namespace Project_Group3_SWD.Controllers
{
	[Controller]
	[Route("Order")]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly IGHNService _ghnService;
		public OrderController(IOrderService orderService, IGHNService ghnService)
		{
			_orderService = orderService;
			_ghnService = ghnService;
		}
		[HttpGet]
		[Route("Index")]
		public async Task<IActionResult> GetAllOrder()
		{
			User user = Extensions.SessionExtensions.GetObjectFromSession<User>(HttpContext.Session, "user");
			List<Order> orders = await _orderService.GetByUserId(user.Id);
			List<OrderGHNViewModel> orderGHN = await _ghnService.GetAllOrders();
			List<OrderGHNViewModel> filterOrderGHN = orderGHN.Where(x => orders.Any(y => y.OrderCode == x.OrderCode)).ToList();
			ViewBag.Orders = filterOrderGHN;
			return View("~/Views/Order/Index.cshtml");
		}
		[HttpGet]
		public async Task<IActionResult> Detail([FromQuery] string orderCode)
		{
			var order = await _ghnService.GetOrderDetailByOrderCode(orderCode);
			ViewBag.Order = order;
			return View("~/Views/Order/Detail.cshtml");
		}


	}
}
