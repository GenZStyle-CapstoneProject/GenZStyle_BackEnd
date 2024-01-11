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
    public class HashTagDAO
    {
        private GenZStyleDbContext _dbContext;
        public HashTagDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Search By HashTagName
        public async Task<List<Hashtag>> SearchByHashTagName(string hashtagname)
        {
            try
            {
                List<Hashtag> hashtags = await _dbContext.Hashtags
                    .Include(u => u.HashPosts)
                    .Where(a => a.Name.Contains(hashtagname))
                    .ToListAsync();

                return hashtags;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
