using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DAL.Entities.Order_Aggregate
{
    public class Address
    {
        public Address(string fristName, string lastName, string country, string city, string street)
        {
            FristName = fristName;
            LastName = lastName;
            Country = country;
            City = city;
            Street = street;
        }
        public Address()
        {
            
        }

        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
