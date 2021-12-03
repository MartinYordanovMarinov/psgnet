using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepos;
         private readonly IGenericRepository<ProductBrand> _productBrandRepos;
          private readonly IGenericRepository<ProductType> _productTypeRepos;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepos,
        IGenericRepository<ProductBrand> productBrandRepos,IGenericRepository<ProductType> productTypeRepos, IMapper mapper )
        {
            _mapper = mapper;
            _productTypeRepos = productTypeRepos;
            _productBrandRepos = productBrandRepos;
            _productsRepos = productsRepos;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {

           var spec = new ProductsWithTypesAndBrandsSpecification();
           var products = await _productsRepos.ListAsync(spec);

           return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }
         [HttpGet("{id}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepos.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductToReturnDto>(product);
        }
        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
           return Ok(await _productBrandRepos.ListAllAsync()); 

        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
           return Ok(await _productTypeRepos.ListAllAsync());

        }
    }
}