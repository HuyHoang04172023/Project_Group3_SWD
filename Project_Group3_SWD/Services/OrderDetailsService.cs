using Project_Group3_SWD.Models;
using Project_Group3_SWD.Repositories;

namespace Project_Group3_SWD.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        public OrderDetailsService(IOrderDetailsRepository orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        public OrderDetail GetProductById(int id, int orderId)
        {
           return _orderDetailsRepository.GetProductById(id, orderId);
        }

        public void RemoveProductById(int id, int orderId)
        {
            _orderDetailsRepository.RemoveProductById(id, orderId);
        }

        public void UpdateProductQuantity(OrderDetail orderDetail)
        {
            _orderDetailsRepository.UpdateProductQuantity(orderDetail);
        }

    }
}
