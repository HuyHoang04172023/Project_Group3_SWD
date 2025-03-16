using Project_Group3_SWD.Models;
using Project_Group3_SWD.ViewModels;

namespace Project_Group3_SWD.Repositories
{
    public interface IOrderRepository
    {
        public void AddOrder(Order order, List<Item> cart);
        public List<Order> GetAll();
        public List<Order> GetByUserId(int userId);
        public Order GetById(int id);
        public void UpdateOrder(Order order);
    }
}
