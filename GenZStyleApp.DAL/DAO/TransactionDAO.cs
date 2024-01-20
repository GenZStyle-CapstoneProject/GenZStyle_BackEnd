using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class TransactionDAO
    {
        private GenZStyleDbContext _dbContext;
        public TransactionDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region Create wallet transaction
        public async Task CreateWalletTransactionAsync(Transaction wallet)
        {
            try
            {
                await this._dbContext.Transactions.AddAsync(wallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
