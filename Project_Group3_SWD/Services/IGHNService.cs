using Project_Group3_SWD.ViewModels;

namespace Project_Group3_SWD.Services
{
	public interface IGHNService
	{
		Task<OrderGHNViewModel> GetOrderDetailByOrderCode(string orderCode);

		Task<List<OrderGHNViewModel>> GetAllOrders();
	}
}
