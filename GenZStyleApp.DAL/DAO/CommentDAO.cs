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
    public class CommentDAO
    {
        private GenZStyleDbContext _dbContext;
        public CommentDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddCommentAsync(Comment newComment)
        {
            try
            {
                await this._dbContext.Comments.AddAsync(newComment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Comment>> GetCommentByPostIdAsync(int postid)
        {
            try
            {
                return await _dbContext.Comments.Include(c => c.Account)
                                                 .Include(c => c.Post)
                                                 .Where(c => c.PostId == postid)
                                                 .ToListAsync();
                                                 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}



