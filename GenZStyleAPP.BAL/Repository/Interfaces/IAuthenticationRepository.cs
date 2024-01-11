﻿using BMOS.BAL.DTOs.Authentications;
using BMOS.BAL.DTOs.JWT;
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
        public Task<PostLoginResponse> LoginAsync(GetLoginRequest account, JwtAuth jwtAuth);


        public Task<PostRecreateTokenResponse> ReCreateTokenAsync(PostRecreateTokenRequest request, JwtAuth jwtAuth);

    }
}
