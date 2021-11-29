using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _repos;
        public ProductsController(IProductRepository repos)
        {
            
            _repos = repos;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
           var products = await _repos.GetProductsAsync();

           return Ok(products);

        }
         [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            return await _repos.GetProductByIdAsync(id);
        }
        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
           return Ok(await _repos.GetProductBrandsAsync());

        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
           return Ok(await _repos.GetProductTypesAsync());

        }
    }
}