using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface  ITransactionRepository
    {
        public Task<GetTransactionResponse> CreateWalletTransactionAsync(PostTransactionRequest model, MomoConfigModel _config);
    }
}
