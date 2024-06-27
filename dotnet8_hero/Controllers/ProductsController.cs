using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet8_hero.Data;
using dotnet8_hero.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetProducts()
        {
            var products = this.DatabaseContext.Products.ToList();
            return Ok(products);
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

            var selectedProduct = this.DatabaseContext.Products.Select(ProductResponse.FromProduct).Where(p => p.ProductId == id).FirstOrDefault();
            return Ok(selectedProduct);

        }





    }
}