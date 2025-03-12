using A_LIÊM_SHOP.Models;
using A_LIÊM_SHOP.ViewModels;

namespace A_LIÊM_SHOP.Services
{
    public interface IOrderService
    {
        public void AddOrder(Order order, List<Item> cart);
        public List<Order> GetAll();
        public Order GetById(int id);
        public List<Order> GetByUserId(int userId);
        public void UpdateOrder(Order order);
    }
}
