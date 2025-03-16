using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;

        public OrderDetailsRepository(WebkinhdoanhquanaoContext context)
        {
            _context = context;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }

        public void GetOrderDetailByOrderId(int orderId)
        {
            _context.OrderDetails.Find(orderId);
        }

        public OrderDetail GetProductById(int id, int orderId)
        {
            return _context.OrderDetails
                  .FirstOrDefault(od => od.OrderId == orderId && od.ProductId == id);
        }

        public void RemoveProductById(int id, int orderId)
        {
           _context.OrderDetails.Remove(GetProductById(id, orderId));
           _context.SaveChanges();
        }

        public void UpdateProductQuantity(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            _context.SaveChanges();
        }

        


    }
}
