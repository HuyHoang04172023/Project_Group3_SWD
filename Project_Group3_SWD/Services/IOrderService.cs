using Project_Group3_SWD.Models;
using Project_Group3_SWD.ViewModels;

namespace Project_Group3_SWD.Services
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
