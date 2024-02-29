using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class UserRelationDAO
    {
        private GenZStyleDbContext _dbContext;
        public UserRelationDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddFollowAsync(UserRelation userRelation)
        {
            try 
            {
                
                await _dbContext.AddAsync(userRelation);
            }catch (Exception ex) 
            {
                    throw new Exception(ex.Message);
            }

        }

        public async Task<List<UserRelation>> GetFollower (int userId)
        {
            try
            {

                return await _dbContext.UserRelations.Include(u => u.Account)
                                              .Where(u => u.FollowingId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserRelation>> GetFollowing(int userId)
        {
            try
            {

                return await _dbContext.UserRelations.Include(u => u.Account)
                                              .Where(u => u.FollowerId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
