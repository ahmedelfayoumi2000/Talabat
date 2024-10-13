using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DAL.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int ProductBrandId { get; set; }
        public ProductBrand productBrand { get; set; } //Navigation Property

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } //Navigation Property
    }

}
