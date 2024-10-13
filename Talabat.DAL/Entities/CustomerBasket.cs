using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DAL.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public CustomerBasket()
        {

        }
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
