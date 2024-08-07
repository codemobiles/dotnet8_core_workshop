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
using Microsoft.AspNetCore.Authorization;
//using dotnet8_hero.Models;

namespace dotnet8_hero.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin, Cashier")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public IProductService ProductService { get; }
        public ILogger<ProductsController> Logger { get;set; }
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            this.Logger = logger;
            this.ProductService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsAsync()
        {
            return (await this.ProductService.FindAll()).Select(ProductResponse.FromProduct).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int id)
        {
            Logger.LogInformation("Calling GetProductByIdAsync");
            Logger.LogDebug("Debugging GetProductByIdAsync");
            var product = await this.ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }

            return ProductResponse.FromProduct(product);

        }


        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Search([FromQuery] string name)
        {
            var result = (await this.ProductService.Search(name))
            .Select(ProductResponse.FromProduct)
            .ToList();

            return result;
        }


        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync([FromForm] ProductRequest productRequest)
        {
            string finalImageName = "";
            if (productRequest.FormFiles != null)
            {
                (string errorMessage, string imageName) = await ProductService.UploadImage(productRequest.FormFiles);
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest();
                }
                finalImageName = imageName;
            }


            var product = productRequest.Adapt<Product>();
            product.Image = finalImageName;
            await ProductService.Create(product);
            return StatusCode((int)HttpStatusCode.Created, product);
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
    }
}