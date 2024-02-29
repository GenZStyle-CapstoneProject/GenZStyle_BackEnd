using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class WalletDAO
    {
        private GenZStyleDbContext _dbContext;
        public WalletDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task CreateWalletAsync(Wallet wallet)
        {
            try
            {
                await this._dbContext.Wallets.AddAsync(wallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region Get wallet by accountId
        public async Task<Wallet> GetWalletByAccountIdAsync(int accountId)
        {
            try
            {
                return await this._dbContext.Wallets.Where(a => a.Account.AccountId == accountId)
                                                                            .SingleOrDefaultAsync();
                                                        

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public void UpdateWallet(Wallet wallet)
        {
            try
            {
                this._dbContext.Entry<Wallet>(wallet).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
