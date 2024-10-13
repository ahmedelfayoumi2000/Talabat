using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications.product;
using Talabat.DAL.Entities;

namespace Talabat.BLL.Specifications.Product_Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecPrams productPrams)
            : base(p =>
            (string.IsNullOrEmpty(productPrams.Search) || p.Name.ToLower().Contains(productPrams.Search)) &&
            (!productPrams.TypeID.HasValue || p.ProductTypeId == productPrams.TypeID.Value) &&
            (!productPrams.BrandID.HasValue || p.ProductBrandId == productPrams.BrandID.Value)
            )
        {

        }
    }
}
