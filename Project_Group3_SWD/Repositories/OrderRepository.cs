using Project_Group3_SWD.Models;
using Project_Group3_SWD.ViewModels;
using Project_Group3_SWD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Project_Group3_SWD.Proxy;


namespace Project_Group3_SWD.Repositories
{
    public class OrderRepository : Controller, IOrderRepository
    {
        private readonly WebkinhdoanhquanaoContext _context;
        private readonly GHNService _shippingService;

        public OrderRepository(WebkinhdoanhquanaoContext context, GHNService shippingService)
        {
            _context = context;
            _shippingService = shippingService;
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
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order, List<Item> cart)
        {
            try
            {
                var orderDTO = new OrderViewModel
                {
                    PaymentTypeId = "2", // Assuming PaymentTypeId is not in Order or Item
                    Note = order.Note ?? "Tintest 123",
                    RequiredNote = "KHONGCHOXEMHANG", // Assuming RequiredNote is not in Order or Item
                    FromName = "TinTest124",
                    FromPhone = "0987654321",
                    FromAddress = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
                    FromWardName = "Phường 14", // Assuming FromWardName is not in Order or Item
                    FromDistrictName = "Quận 10", // Assuming FromDistrictName is not in Order or Item
                    FromProvinceName = "HCM", // Assuming FromProvinceName is not in Order or Item
                    ReturnPhone = "0332190444", // Assuming ReturnPhone is not in Order or Item
                    ReturnAddress = "39 NTT", // Assuming ReturnAddress is not in Order or Item
                    ReturnDistrictId = null, // Assuming ReturnDistrictId is not in Order or Item
                    ReturnWardCode = "", // Assuming ReturnWardCode is not in Order or Item
                    ClientOrderCode = "", // Assuming ClientOrderCode is not in Order or Item
                    ToName = order.Name ,
                    ToPhone = order.Phone ,
                    ToAddress = order.Address ,
                    ToWardCode = "20308", // Assuming ToWardCode is not in Order or Item
                    ToDistrictId = "20308", // Assuming ToDistrictId is not in Order or Item
                    CodAmount = "20308", // Assuming CodAmount is not in Order or Item
                    Content = "Theo New York Times", // Assuming Content is not in Order or Item
                    Weight = "20308", // Assuming Weight is not in Order or Item
                    Length = "20308", // Assuming Length is not in Order or Item
                    Width = "20308", // Assuming Width is not in Order or Item
                    Height = "20308", // Assuming Height is not in Order or Item
                    PickStationId = "20308", // Assuming PickStationId is not in Order or Item
                    DeliverStationId = null, // Assuming DeliverStationId is not in Order or Item
                    InsuranceValue = "20308", // Assuming InsuranceValue is not in Order or Item
                    ServiceId = "20308", // Assuming ServiceId is not in Order or Item
                    ServiceTypeId = "20308", // Assuming ServiceTypeId is not in Order or Item
                    Coupon = null, // Assuming Coupon is not in Order or Item
                    PickShift = new List<String> { "20308" }, // Assuming PickShift is not in Order or Item
                    Items = cart.Select(item => new OrderItemViewModel
                    {
                        Name = item.Product.Name,
                        Code = item.Product.Id.ToString(), // Assuming Code is the Product Id
                        Quantity = item.Quantity.ToString(),
                        Price = item.Product.Price.ToString(),
                        Length = "20308", // Assuming Length is not in Item
                        Width = "20308", // Assuming Width is not in Item
                        Height = "20308", // Assuming Height is not in Item
                        Weight = "20308", // Assuming Weight is not in Item
                        Category = new ItemCategoryViewModel
                        {
                            Level1 = "Áo" // Assuming Category is not in Item
                        }
                    }).ToList()
                };

                //var response = await _shippingService.CreateShippingOrderAsync(orderDTO);
                //return Ok(response);

				AddOrder(order, cart);
				return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

        public async  Task<List<Order>> GetByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
