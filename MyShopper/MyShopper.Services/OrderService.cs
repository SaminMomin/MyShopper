using MyShopper.core.Contracts;
using MyShopper.core.Models;
using MyShopper.core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopper.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> orderContext)
        {
            this.orderContext = orderContext;
        }
        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach(var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image=item.Image,
                    Price=item.Price,
                    ProductName=item.Productname,
                    Quantity=item.Quantity
                    
                });
                orderContext.Insert(baseOrder);
                orderContext.Commit();
            }
        }
    }
}
