using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.DAL.Entities.Order_Aggregate;

namespace Talabat.BLL.Specifications.Order_Specifications
{
    public class OrderWithItemAndDeliveryMethodSpecifications : BaseSpecification<Order>
    {
        // This Constructor is Used For Get All Orders For A Specific User
        public OrderWithItemAndDeliveryMethodSpecifications(string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail)
        {
            AddIncludes(O => O.Items);
            AddIncludes(O => O.DeliveryMethod);

            AddOrderByDescending(O => O.OrderDate);
        }

        // This Constructor is Used For Get An Order For a Specific User
        public OrderWithItemAndDeliveryMethodSpecifications(int orderId, string buyerEmail)
            : base(O => (O.BuyerEmail == buyerEmail && O.Id == orderId))
        {
            AddIncludes(O => O.Items);
            AddIncludes(O => O.DeliveryMethod);

        }


    }
}
