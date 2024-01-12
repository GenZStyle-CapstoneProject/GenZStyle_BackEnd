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
    public class FashionItemDAO
    {
        private GenZStyleDbContext _dbContext;
        public FashionItemDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Search By Fashioname
        public async Task<List<FashionItem>> SearchByFashionName(string fashioname)
        {
            try
            {
                List<FashionItem> fashionames = await _dbContext.FashionItems
                    .Where(a => a.fashionName.Contains(fashioname))
                    .ToListAsync();

                return fashionames;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
