using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Account;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleApp.DAL.Models;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMOS.DAL.Enums;
using GenZStyleAPP.BAL.Repository.Interfaces;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UserRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserResponse> Register( RegisterRequest registerRequest)
        {
            try
            {
                var role = await _unitOfWork.RoleDAO.GetRoleAsync((int)RoleEnum.Role.User);
                var customerByEmail = await _unitOfWork.UserDAO.GetUserByEmailAsync(registerRequest.Email);
                if (customerByEmail != null)
                {
                    throw new BadRequestException("Email already exist in the system.");
                }

                var customerPhone = await _unitOfWork.UserDAO.GetUserByPhoneAsync(registerRequest.Phone);
                if (customerPhone != null)
                {
                    throw new BadRequestException("Phone already exist in the system.");
                }

                // assign registerRequest to account
                Account account = new Account
                {
                    Email = registerRequest.Email,
                    PasswordHash = registerRequest.PasswordHash,
                    Firstname = registerRequest.FirstName,
                    Lastname = registerRequest.LastName,
                    Username = registerRequest.UserName,
                    IsActive = true,
                    IsVip = true,
                };
                await _unitOfWork.AccountDAO.AddAccountAsync(account);

                // assign registerRequest to customer
                User user = new User
                {
                    City = registerRequest.City,
                    AvatarUrl = registerRequest.Avatar,
                    Address = registerRequest.Address,
                    Dob = registerRequest.Dob,
                    Gender = registerRequest.Gender,
                    Phone = registerRequest.Phone,
                    Role = role,
                    
                };

                user.Accounts = new List<Account>();
                user.Accounts.Add(account);
                Wallet wallet = new Wallet()
                    {
                        Account = account,
                        Balance = 0
                    };
                await _unitOfWork.UserDAO.AddNewUser(user);
                await this._unitOfWork.WalletDAO.CreateWalletAsync(wallet);

                //Save to Database
                await _unitOfWork.CommitAsync();

                

                return new GetUserResponse
                {
                    Phone = user.Phone,
                    UserID = user.UserId,
                    Address = user.Address,
                    Avatar = user.AvatarUrl,
                    BirthDate = user.Dob,                    
                    Gender = user.Gender,
                    Account = _mapper.Map<GetAccountResponse>(user.Accounts),
                };
            }
            catch (BadRequestException ex)
            {
                string fieldNameError = "";
                if (ex.Message.ToLower().Contains("email"))
                {
                    fieldNameError = "Email";
                }
                else if (ex.Message.ToLower().Contains("phone"))
                {
                    fieldNameError = "Phone";
                }
                string error = ErrorHelper.GetErrorString(fieldNameError, ex.Message);
                throw new BadRequestException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

    }
}
