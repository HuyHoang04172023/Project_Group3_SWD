using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.Repositories;
using A_LIÊM_SHOP.ViewModels;

namespace A_LIÊM_SHOP.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public void AddOrder(Order order, List<Item> cart)
        {
            _orderRepository.AddOrder(order, cart);
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public List<Order> GetByUserId(int userId)
        {
            return _orderRepository.GetByUserId(userId);
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.UpdateOrder(order);
        }
    }
}
