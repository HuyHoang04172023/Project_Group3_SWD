using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Services
{
    public interface IOrderDetailsService
    {
        void UpdateProductQuantity(OrderDetail orderDetail);
        OrderDetail GetProductById(int id, int orderId);

        void RemoveProductById(int id, int orderId);
    }
}
