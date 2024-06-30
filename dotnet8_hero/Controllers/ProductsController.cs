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
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public DatabaseContext DatabaseContext { get; set; }
        public ProductsController(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var products = this.DatabaseContext.Products.Include(p => p.Category).Select(ProductResponse.FromProduct).ToList();
            return products;
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            // var product = this.DatabaseContext.Products.Find(id);
            // return Ok(product);

            // var selectedProduct = this.DatabaseContext.Products.Find(id);
            // if (selectedProduct != null)
            // {
            //     var product = ProductResponse.FromProduct(selectedProduct);
            //     return Ok(product);
            // }
            // return NotFound();

            var selectedProduct = this.DatabaseContext.Products.Include(p => p.Category).Select(ProductResponse.FromProduct).Where(p => p.ProductId == id).FirstOrDefault();
            return Ok(selectedProduct);

        }


        [HttpGet("search")]
        public ActionResult<IEnumerable<ProductResponse>> Search([FromQuery] string name)
        {
            // search
            var result = this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.ToLower().Contains(name.ToLower())).Select(ProductResponse.FromProduct).ToList();
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