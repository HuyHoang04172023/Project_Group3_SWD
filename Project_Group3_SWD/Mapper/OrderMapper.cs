using Project_Group3_SWD.ViewModels;

namespace Project_Group3_SWD.Mapper
{
	public class OrderMapper
	{
		public static OrderViewModel MapToOrderViewModel(OrderGHNViewModel order)
		{
			OrderViewModel orderViewModel = new OrderViewModel();
			orderViewModel.Name = order.ToName;
			orderViewModel.Address = order.ToAddress;
			orderViewModel.Phone = order.ToPhone;
			return orderViewModel;
		}
	}
}
