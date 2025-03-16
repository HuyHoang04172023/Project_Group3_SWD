using Project_Group3_SWD.Models;
using Project_Group3_SWD.Repositories;
using Project_Group3_SWD.ViewModels;

namespace Project_Group3_SWD.Services
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
           // _orderRepository.AddOrder(order, cart);
            _orderRepository.CreateOrder(order, cart);
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public async Task<List<Order>> GetByUserId(int userId)
        {
            return await _orderRepository.GetByUserId(userId);
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.UpdateOrder(order);
        }
    }
}
