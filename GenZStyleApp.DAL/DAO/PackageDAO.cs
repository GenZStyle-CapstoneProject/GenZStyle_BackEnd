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
    public class PackageDAO
    {
        private GenZStyleDbContext _dbContext;
        public PackageDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            try 
            { 
                return await _dbContext.Packages.Include(p => p.Invoices)
                                                .FirstOrDefaultAsync(p => p.PackageId == packageId);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
