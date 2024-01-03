using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class UserDAO
    {
        private GenZStyleDbContext _dbContext;
        public UserDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region Get User by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Users.Include(c => c.Accounts)
                                                 .SingleOrDefaultAsync(c => c.Accounts.Any(a => a.Email.Equals(email)
                                                 && c.Accounts.Any(a => a.IsActive == true)
                                                 ))
                                                  ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get User by phone
        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            try
            {
                return await _dbContext.Users.Include(c => c.Accounts)
                                                 .SingleOrDefaultAsync(c => c.Phone.Equals(phone));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task AddNewUser(User User)
        {
            try
            {
                await _dbContext.Users.AddAsync(User);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private readonly GenZStyleDbContext _context = new GenZStyleDbContext();

        //get User
        public async Task<List<User>> GetAllUser()
        {
            try
            {
                List<User> users = await _dbContext.Users.ToListAsync();
                return users;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Users
                    //.Include(p => p.ProductImages)
                    //.Include(p => p.ProductMeals)
                    //.ThenInclude(p => p.Meal)
                    //.ThenInclude(m => m.MealImages)
                   .SingleOrDefaultAsync(p => p.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Update user
        public void UpdateUser(User user)
        {
            try
            {
                this._dbContext.Entry<User>(user).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        public void DeleteUser(int id)
        {
            var user = _context.Users.Where(c => c.UserId == id).ToList()[0];
            var isCarInRenting = _context.Accounts.Where(c => c.UserId == id).ToList().Count > 0;
            if (isCarInRenting) // have in rent --> Update status
            {
                //user.CarStatus = 0;
                user.Role = null;
                

                _context.Users.Update(user);
                _context.SaveChanges();
            }
            else // Not have in rent --> Remove
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
