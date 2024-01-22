using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class ProductDAO
    {
        private GenZStyleDbContext _dbContext;
        public ProductDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //get all product
        public async Task<List<Product>> GetAllProducts()
        {
            try
            {

                List<Product> products = await _dbContext.Products.ToListAsync();
                return products;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Add new Product
        public async Task AddNewProduct(Product product)
        {
            try
            {
                await _dbContext.Products.AddAsync(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
