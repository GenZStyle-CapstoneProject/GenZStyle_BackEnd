using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

        #region AllHashtag
        public async Task<List<Hashtag>> GetAllHashTag()
        {
            try
            {
                return await _dbContext.Hashtags.Include(h => h.HashPosts).ThenInclude(J => J.Post)
                                                .ToListAsync();         

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Create Hashtag
        public async Task CreateHashTagAsync(Hashtag hashtag)
        {
            try
            {
                await this._dbContext.Hashtags.AddAsync(hashtag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get customer by name
        public async Task<Hashtag> GetHashTagByNameAsync(string name)
        {
            try
            {
                return await _dbContext.Hashtags.Include(c => c.HashPosts).ThenInclude(h => h.Post)
                                                 .SingleOrDefaultAsync(c => c.Name.Equals(name));
                                                 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


    }
}
