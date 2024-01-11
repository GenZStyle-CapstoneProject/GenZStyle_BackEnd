using GenZStyleAPP.BAL.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IAccountRepository
    {
        public Task<GetAccountResponse> ChangPassword(int accountId, ChangePasswordRequest changePasswordRequest);
        public Task<List<GetAccountResponse>> SearchByUserName(string username);
    }
}
