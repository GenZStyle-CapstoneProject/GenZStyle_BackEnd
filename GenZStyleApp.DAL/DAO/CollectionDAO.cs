using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class CollectionDAO
    {
        private GenZStyleDbContext _dbContext;
        public CollectionDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Add new collection
        public async Task AddNewCollection(Collection collection)
        {
            try
            {
                await _dbContext.Collections.AddAsync(collection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
