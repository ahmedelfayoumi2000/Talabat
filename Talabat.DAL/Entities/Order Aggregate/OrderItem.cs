using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DAL.Entities.Order_Aggregate
{
    public class OrderItem :BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrder itemOrdered, decimal price, int quanity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quanity = quanity;
        }

        public ProductItemOrder ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quanity { get; set; }

    }
}
