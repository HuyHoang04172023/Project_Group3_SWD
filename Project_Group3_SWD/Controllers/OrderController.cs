using Project_Group3_SWD.Extensions;
using Project_Group3_SWD.Mapper;
using Project_Group3_SWD.Models;
using Project_Group3_SWD.Services;
using Project_Group3_SWD.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Project_Group3_SWD.Controllers
{
	[Controller]
	[Route("order")]
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
		[Route("index")]
		public async Task<IActionResult> GetAllOrder()
		{
			
			List<OrderGHNViewModel> orders = await _ghnService.GetAllOrders();
			ViewBag.Orders = orders;
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
