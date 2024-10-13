using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.DAL.Entities;
using Talabat.DAL.Entities.Order_Aggregate;

namespace Talabat.BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        //Task<Order> UpdateOrderPaymentSucceded(string paymantIntentId);
        //Task<Order> UpdateOrderPaymentFailed(string paymantIntentId);

        Task<Order> UpdateOrderPaymentStatus(string paymentIntentId, OrderStatus status);
    }
}
