using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.BLL.Interfaces;
using Talabat.BLL.Specifications;
using Talabat.BLL.Specifications.product;
using Talabat.BLL.Specifications.Product_Specifications;
using Talabat.DAL.Data;
using Talabat.DAL.Entities;

namespace Talabat.API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductType> _typesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> brandsRepo,
            IGenericRepository<ProductType> typesRepo, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _brandsRepo = brandsRepo;
            _typesRepo = typesRepo;
            _mapper = mapper;
        }

        [CachedResponse(600)] 
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecPrams productParams)
         
        
        {
            //var products = await _productsRepo.GetAllAsync();

            var spec = new ProductWithTypeAndBrandSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.GetCountAsync(countSpec);

            var products = await _productsRepo.GetAllWithSpecAsync(spec);

            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);


            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, Data));
        }


        [CachedResponse(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypeAndBrandSpecification(id);
            var product = await _productsRepo.GetByIdWithSpecAsync(spec);

            var productDto = _mapper.Map<Product, ProductToReturnDto>(product);
            if (productDto == null) return NotFound(new ApiResponse(404));
            else return Ok(productDto);

            //return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [CachedResponse(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var prands = await _brandsRepo.GetAllAsync();

            if (prands == null) return NotFound(new ApiResponse(404));
            else return Ok(prands);

        }

        [CachedResponse(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
        {
            var types = await _typesRepo.GetAllAsync();

            if (types == null) return NotFound(new ApiResponse(404));
            else return Ok(types);
        }


    }


}
