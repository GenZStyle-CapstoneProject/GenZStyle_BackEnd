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
    public class PostDAO
    {
        private GenZStyleDbContext _dbContext;
        public PostDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Post> GetPostById(int Id)
        {
            try
            {
                return await _dbContext.Posts.Include(a => a.Account)
                                             .SingleOrDefaultAsync(a => a.PostId == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Post>> GetPosts()
        {
            try 
            {
                return await _dbContext.Posts .Include(a => a.Account)
                                              .Include(a => a.Likes)
                                              .Include(a => a.HashPosts)
                                              .ToListAsync();
                                                

            }catch (Exception ex) 
    {
                throw new Exception(ex.Message);
            }
        }
        public void UpdatePostComment(Post post)
        {
            try
            {
                this._dbContext.Entry<Post>(post).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
