using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet8_hero.Data;
using dotnet8_hero.DTOs.Product;
using dotnet8_hero.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using dotnet8_hero.Interfaces;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public DatabaseContext DatabaseContext { get; set; }
        public IProductService ProductService { get; }
        public ProductsController(DatabaseContext databaseContext, IProductService productService)
        {
            this.ProductService = productService;
            this.DatabaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            return (await this.ProductService.FindAll()).Select(ProductResponse.FromProduct).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int id)
        {
            var product = await this.ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            return ProductResponse.FromProduct(product);

        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Search([FromQuery] string name)
        {
            var result = (await this.ProductService.Search(name)).Select(ProductResponse.FromProduct).ToList();
            return result;
        }


        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromForm] ProductRequest productRequest)
        {

            // (string errorMessage, string imageName) = await ProductService.UploadImage(productRequest.FormFiles);
            // if (!String.IsNullOrEmpty(errorMessage))
            // {
            //     return BadRequest();
            // }

            var product = productRequest.Adapt<Product>();
            product.Image = "";
            await ProductService.Create(product);
            return StatusCode((int)HttpStatusCode.Created, product);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletProductAsync(int id)
        {
            var product = await ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            await ProductService.Delete(product);
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromForm] ProductRequest productRequest)
        {
            if (id != productRequest.ProductId)
            {
                return BadRequest();
            }

            var product = await ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            // (string errorMessage, string imageName) = await productService.UploadImage(productRequest.FormFiles);
            // if (!String.IsNullOrEmpty(errorMessage))
            // {
            //     return BadRequest();
            // }
            // if (!String.IsNullOrEmpty(imageName))
            // {
            //     product.Image = imageName;
            // }

            productRequest.Adapt(product);
            await ProductService.Update(product);
            return Ok(ProductResponse.FromProduct(product));

        }
    }
}