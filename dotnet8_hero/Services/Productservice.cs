using dotnet8_hero.Data;
using dotnet8_hero.Entities;
using dotnet8_hero.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_hero.Services
{
    public class Productservice : IProductService
    {
        public DatabaseContext DatabaseContext { get; }

        public Productservice(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;

        }

        public async Task<IEnumerable<Product>> FindAll()
        {
            var products = await this.DatabaseContext.Products.Include(p => p.Category).ToListAsync();
            return products;
        }

        public Task Create(Product product)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Product product)
        {
            throw new NotImplementedException();
        }


        public Task<Product> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> Search(string name)
        {
            throw new NotImplementedException();
        }

        public Task Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}