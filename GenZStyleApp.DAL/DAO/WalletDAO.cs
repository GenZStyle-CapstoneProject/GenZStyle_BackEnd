using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
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

    }
}
