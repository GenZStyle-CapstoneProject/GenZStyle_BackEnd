using AutoMapper;
using BMOS.BAL.Helpers;
using GenZStyleApp.DAL.DAO;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.Heplers;
using GenZStyleAPP.BAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AccountRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetAccountResponse> ChangPassword(int accountId, ChangePasswordRequest changPasswordRequest)
        {
            try
            {
                var account = await _unitOfWork.AccountDAO.GetAccountById(accountId);
                if (account == null)
                {
                    throw new NotFoundException("AccountId does not exist in system.");
                }

                if (changPasswordRequest.OldPassword != account.PasswordHash)
                {
                    throw new BadRequestException("Old password does not match with current password.");
                }

                if (changPasswordRequest.NewPassword != changPasswordRequest.ConfirmPassword)
                {
                    throw new BadRequestException("New password and old password do not match each other.");
                }
                account.PasswordHash = changPasswordRequest.NewPassword;
                _unitOfWork.AccountDAO.ChangePassword(account);
                _unitOfWork.Commit();
                return _mapper.Map<GetAccountResponse>(account);
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

        public async Task<List<GetAccountResponse>> SearchByUserName(string username)
        {
            try
            {
                
                // Sử dụng hàm SearchByUsername từ AccountDAO
                List<Account> accounts = await _unitOfWork.AccountDAO.SearchByUsername(username);

                // Chuyển đổi List<Account> thành List<AccountDTO> nếu cần thiết
                List<GetAccountResponse> accountDTOs = _mapper.Map<List<GetAccountResponse>>(accounts);

                return accountDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
