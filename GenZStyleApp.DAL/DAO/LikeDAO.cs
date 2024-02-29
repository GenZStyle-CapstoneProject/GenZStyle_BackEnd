using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class LikeDAO
    {
        private GenZStyleDbContext _dbContext;

        public LikeDAO(GenZStyleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //get likes
        public async Task<List<Like>> GetAllAccountByLikes(int postId)
        {
            try
            {
                List<Like> likes = await _dbContext.Likes
                    .Include(l => l.Account).ThenInclude(l => l.User)
                    .Where(l => l.PostId == postId)
                    .ToListAsync();

                return likes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddLikeAsync(Like like)
        {
            try
            {
                await this._dbContext.Likes.AddAsync(like);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ChangeLike(Post post)
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

        public async Task<List<Like>> GetLikeByPostId(int PostId)
        {
            try
            {
                return await this._dbContext.Likes.Include(l => l.Post)
                .Where(l => l.PostId == PostId)
                .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


    }
}
