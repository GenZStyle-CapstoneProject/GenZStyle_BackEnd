using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class AccountDAO
    {
        private GenZStyleDbContext _dbContext;
        public AccountDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Account> GetAccountAsync(int emailAddress)
        {
            try
            {
                return await this._dbContext.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(emailAddress));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAccountAsync(Account newAccount)
        {
            try
            {
                await this._dbContext.Accounts.AddAsync(newAccount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> LoginAsync(string email, string password)
        {
            try
            {
                return await this._dbContext.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(email)
                                                                                   && x.PasswordHash.Equals(password)
                                                                                   && x.IsActive == true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
