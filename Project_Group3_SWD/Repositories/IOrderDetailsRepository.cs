using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Repositories
{
    public interface IOrderDetailsRepository
    {
        void AddOrderDetail(OrderDetail orderDetail);
        void GetOrderDetailByOrderId(int orderId);
        void UpdateProductQuantity(OrderDetail orderDetail);

        OrderDetail GetProductById(int id, int orderId);

        void RemoveProductById(int id, int orderId);
    }
}
