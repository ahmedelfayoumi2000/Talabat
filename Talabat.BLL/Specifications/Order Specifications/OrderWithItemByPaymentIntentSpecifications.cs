using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.DAL.Entities.Order_Aggregate;

namespace Talabat.BLL.Specifications.Order_Specifications
{
    public class OrderWithItemByPaymentIntentSpecifications : BaseSpecification<Order>
    {
        public OrderWithItemByPaymentIntentSpecifications(string paymentIntentId)
            :base(Order => Order.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
