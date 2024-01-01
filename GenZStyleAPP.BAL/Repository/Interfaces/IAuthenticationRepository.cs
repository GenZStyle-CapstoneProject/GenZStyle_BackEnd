using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Heplers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.BAL.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        public Task<GetLoginResponse> LoginAsync(GetLoginRequest account);
    }
}
