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
        public IActionResult AddProduct([FromForm] ProductRequest productRequest)
        {
            var product = productRequest.Adapt<Product>();
            this.DatabaseContext.Products.Add(product);
            this.DatabaseContext.SaveChanges();
            return StatusCode((int)HttpStatusCode.Created, product);
        }

        // [HttpDelete("{id}")]
        // public IActionResult DeletProduct(int id)
        // {
        //     var product = this.DatabaseContext.Products.Find(id);
        //     if (product == null)
        //     {
        //         return NotFound();
        //     }

        //     this.DatabaseContext.Products.Remove(product);
        //     this.DatabaseContext.SaveChanges();
        //     return NoContent();
        // }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletProductAsync(int id)
        {
            var product = await this.DatabaseContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            this.DatabaseContext.Products.Remove(product);
            await this.DatabaseContext.SaveChangesAsync();
            return NoContent();
        }

    }
}