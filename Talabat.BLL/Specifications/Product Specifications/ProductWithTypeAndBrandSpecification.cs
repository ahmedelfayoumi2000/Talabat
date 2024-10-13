using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Specifications.product;
using Talabat.DAL.Entities;

namespace Talabat.BLL.Specifications.Product_Specifications
{
    public class ProductWithTypeAndBrandSpecification : BaseSpecification<Product>
    {
        //This Constructure is used for get all Products
        public ProductWithTypeAndBrandSpecification(ProductSpecPrams productPrams)
            : base(p =>
            (string.IsNullOrEmpty(productPrams.Search) || p.Name.ToLower().Contains(productPrams.Search))&&
            (!productPrams.TypeID.HasValue || p.ProductTypeId == productPrams.TypeID.Value) &&
            (!productPrams.BrandID.HasValue || p.ProductBrandId == productPrams.BrandID.Value)
            )
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.productBrand);
            AddOrderBy(p => p.Name);

            //PageIndex =2 
            //PageSize = 5 
            ApplyPagination(productPrams.PageSize * (productPrams.PageIndex - 1), productPrams.PageSize);

            if (!string.IsNullOrEmpty(productPrams.Sort))
            {
                switch (productPrams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

        }
        //public ProductWithTypeAndBrandSpecification()
        //{

        //}

        //This Constructure is used for get a specific Product with id
        public ProductWithTypeAndBrandSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.productBrand);
        }
    }
}
