using Project_Group3_SWD.Models;
using Project_Group3_SWD.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Project_Group3_SWD.Repositories
{
    public interface IOrderRepository
    {
        public void AddOrder(Order order, List<Item> cart);
        public List<Order> GetAll();
        Task<List<Order>> GetByUserId(int userId);

        Task<IActionResult> CreateOrder (Order order, List<Item> cart);
        public Order GetById(int id);
        public void UpdateOrder(Order order);
    }
}
