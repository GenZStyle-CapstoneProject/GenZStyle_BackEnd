using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Enums;
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

        public async Task<Account> GetAccountById(int accountId)
        {
            try
            {
                return await _dbContext.Accounts.Include(a => a.User).ThenInclude(u => u.Role)
                                           .Include(a => a.Tokens)
                                           .SingleOrDefaultAsync(a => a.AccountId == accountId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<Account> GetAccountByEmail(string email)
        {

            try
            {
                return await this._dbContext.Accounts
                                    .Include(a => a.User)
                                    .Include(u => u.Posts)
                                    .SingleOrDefaultAsync(a => a.Email.Equals(email) && a.IsActive == true );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Account> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                return await this._dbContext.Accounts.Include(a => a.User).ThenInclude(a => a.Role)
                                                    .FirstOrDefaultAsync(x => x.Username.Equals(email.Trim().ToLower())
                                                                                   && x.PasswordHash.Equals(password)
                                                                                   && x.IsActive == true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateAccountProfile(Account account)
        {
            try
            {
                this._dbContext.Entry<Account>(account).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ChangePassword(Account account)
        {
            try
            {
                this._dbContext.Entry<Account>(account).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Search By UserName
        public async Task<List<Account>> SearchByUsername(string username)
        {
            try
            {
                List<Account> accounts = await _dbContext.Accounts
                    .Include(u => u.Posts)
                    .Where(a => a.Username.Contains(username))
                    .ToListAsync();

                return accounts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        


    }
}
