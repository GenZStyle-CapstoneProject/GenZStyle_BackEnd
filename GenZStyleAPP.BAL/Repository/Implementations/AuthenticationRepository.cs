using AutoMapper;
using GenZStyleApp.DAL.Models;
using Microsoft.Extensions.Configuration;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AuthenticationRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        public async Task<GetLoginResponse> LoginAsync(GetLoginRequest account)
        {
            try
            {
                bool isAdmin = IsAdmin(account);
                if (isAdmin)
                {
                    GetLoginResponse loginResponse = new GetLoginResponse()
                    {
                        UserrID = 0,
                        UserName = account.UserName,
                        FullName = account.UserName,
                        IsAdmin = true
                    };
                    return loginResponse;
                }
                else
                {
                    Account account1 = await this._unitOfWork.AccountDAO.LoginAsync(account.UserName, account.Password);
                    if (account1 == null)
                    {
                        throw new BadRequestException("UserName or Password is invalid.");
                    }
                    GetLoginResponse loginResponse = new GetLoginResponse()
                    {
                        UserrID = account1.AccountId,
                        UserName = account1.Username,
                        FullName = account1.Firstname + account1.Lastname,
                        IsAdmin = false

                    };
                    return loginResponse;
                }
            }
            catch (BadRequestException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new BadRequestException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        private bool IsAdmin(GetLoginRequest account)
        {
            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfiguration configuration = builder.Build();

                string adminEmail = configuration.GetSection("AdminAccount:Email").Value;
                string adminPassword = configuration.GetSection("AdminAccount:Password").Value;
                if (account.UserName.Equals(adminEmail) && account.Password.Equals(adminPassword))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
