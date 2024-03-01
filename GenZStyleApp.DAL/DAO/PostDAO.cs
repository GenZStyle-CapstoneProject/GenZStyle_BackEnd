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

        //get posts
        public async Task<List<Post>> GetPosts()
        {
            try
            {
                
                List<Post> posts = await _dbContext.Posts
                    .OrderByDescending( n => n.CreateTime)
                    .Include(x => x.HashPosts).ThenInclude(u => u.Hashtag)
                    .ToListAsync();
                return posts;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Get post by id
        public async Task<Post> GetPostByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Posts.Include(p => p.Account)
                    
                    .SingleOrDefaultAsync(p => p.PostId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get post by accountid
        public async Task<List<Post>> GetPostByAccountIdAsync(int id)
        {
            try
            {
                return await _dbContext.Posts.Include(p => p.Account)
                                             .Include(p => p.HashPosts).ThenInclude( h => h.Hashtag)
                                             .Where(p => p.AccountId == id)                     
                                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get post by accountid
        public async Task<List<Post>> GetPostByReportAsync(int id)
        {
            try
            {
                // Get posts where the associated reports have IsReport set to true and more than 2 reports
                return await _dbContext.Posts
                    .Where(p => p.PostId == id && p.Reports.Count(r => r.IsReport) >= 2)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Post by Gender
        public async Task<List<Post>> GetPostByGenderAsync(bool gender)
        {
            try
            {
                return await _dbContext.Posts.Include(c => c.Account.User)  // Thêm dòng này để include User
                                             .Where(c => c.Account.User.Gender == gender)
                                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        // Add new Post
        public async Task AddNewPost(Post post)
        {
            try
            {
                await _dbContext.Posts.AddAsync(post);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Update post
        public void UpdatePost(Post post)
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
        #endregion

        public async Task<List<Post>> GetActivePosts()
        {
            try
            {
                List<Post> posts = await _dbContext.Posts.Include(u => u.HashPosts).ThenInclude(x => x.Hashtag)
                                        .ToListAsync();
                return posts;

            }
            catch (Exception ex)
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

        #region DeletePost
        public void DeletePost(Post post)
        {
            try
            {
                this._dbContext.Posts.Remove(post);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
