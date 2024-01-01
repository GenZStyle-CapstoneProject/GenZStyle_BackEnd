using GenZStyleAPP.BAL.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<GetUserResponse> Register(RegisterRequest registerrequest);
    }
}
