using Project_Group3_SWD.Models;
using Project_Group3_SWD.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Project_Group3_SWD.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;

        public OrderRepository(WebkinhdoanhquanaoContext context)
        {
            _context = context;
        }
        public void AddOrder(Order order, List<Item> cart)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            foreach (var item in cart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = Math.Round((decimal)(item.Quantity * item.Product.Price), 2)
                };
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
        }

        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders
                     .Include(x => x.OrderDetails)
                     .ThenInclude(od => od.Product)
                     .FirstOrDefault(x => x.Id == id); 
        }

        public List<Order> GetByUserId(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
