using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Accounts;
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
using System.Collections;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using GenZStyleApp.DAL.Enums;
using GenZStyleAPP.BAL.DTOs.FireBase;

using GenZStyleApp.BAL.Helpers;

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

        public async Task<GetUserResponse> Register(FireBaseImage fireBaseImage,RegisterRequest registerRequest)
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
                    RoleId = 3,
                    Address = registerRequest.Address,
                    Dob = registerRequest.Dob,
                    Gender = registerRequest.Gender,
                    Phone = registerRequest.Phone,
                    Role = role,
                    Accounts = new List<Account> { account },
                };

                

                // Upload image to firebase
                FileHelper.SetCredentials(fireBaseImage);
                FileStream fileStream = FileHelper.ConvertFormFileToStream(registerRequest.Avatar);
                Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Customer");
                user.AvatarUrl = result.Item1;
                
                
                Wallet wallet = new Wallet()
                    {
                        Account = account,
                        Balance = 0,
                        CreatDate = DateTime.Now 
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
                    City = user.City,
                    Gender = user.Gender,
                    Account = _mapper.Map<List<GetAccountResponse>>(user.Accounts),
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
            /*catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }*/
        }

        

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                List<User> users = await this._unitOfWork.UserDAO.GetAllUser();
                return this._mapper.Map<List<User>>(users);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task<User> GetActiveUser(int userId)
        {
            try
            {
                User user = await this._unitOfWork.UserDAO.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException("User does not exist in the system.");
                }
                return this._mapper.Map<User>(user);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("User Id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        #region UpdateUserProfileByAccountIdAsync
        public async Task<User> UpdateUserProfileByAccountIdAsync(int accountId,
                                                                                     FireBaseImage fireBaseImage,
                                                                                     UpdateUserRequest updateUserRequest)
        {
            try
            {
                User user = await _unitOfWork.UserDAO.GetUserByAccountIdAsync(accountId);

                if (user == null)
                {
                    throw new NotFoundException("AccountId does not exist in system");
                }

                user.City = updateUserRequest.City;
                user.Address = updateUserRequest.Address;
                user.Phone = updateUserRequest.Phone;
                user.Gender = updateUserRequest.Gender;
                user.Height = updateUserRequest.Height;
                user.Dob = updateUserRequest.Dob;
                //if (updateCustomerRequest.PasswordHash != null)
                //{
                //    customer.Account.PasswordHash = StringHelper.EncryptData(updateCustomerRequest.PasswordHash);
                //}

                #region Upload image to firebase
                if (updateUserRequest.AvatarUrl != null)
                {
                    FileHelper.SetCredentials(fireBaseImage);
                    //await FileHelper.DeleteImageAsync(user.AvatarUrl, "User");
                    FileStream fileStream = FileHelper.ConvertFormFileToStream(updateUserRequest.AvatarUrl);
                    Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "User");
                    user.AvatarUrl = result.Item1;
                    //customer.AvatarID = result.Item2;
                }
                #endregion

                _unitOfWork.UserDAO.UpdateUser(user);
                await this._unitOfWork.CommitAsync();
                return _mapper.Map<User>(user);
            }

            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("AccountId", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }

        }
        #endregion

        #region DeleteUserAsync
        public async Task DeleteUserAsync(int id, HttpContext httpContext)
        {
            try
            {
                //JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                //string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                //var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                var user = await _unitOfWork.UserDAO.GetUserByIdAsync(id);
                if (user == null)
                {
                    throw new NotFoundException("User id does not exist in the system.");
                }
                //product.Status = (int)ProductEnum.Status.INACTIVE;
                //if (product.ProductMeals != null && product.ProductMeals.Count() > 0)
                //{
                //    foreach (var productMeal in product.ProductMeals)
                //    {
                //        productMeal.Meal.Status = (int)MealEnum.Status.INACTIVE;
                //    }
                //}
                    user.Dob = DateTime.Today;
                //product.ModifiedBy = accountStaff.ID;
                _unitOfWork.UserDAO.DeleteUser(user);
                await _unitOfWork.CommitAsync();

            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("User Id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion


        #region GetUserByAccountIdAsync
        public async Task<User> GetUserByAccountIdAsync(int accountId)
        {
            try
            {
                User user = await _unitOfWork.UserDAO.GetUserByAccountIdAsync(accountId);
                if (user == null)
                {
                    throw new NotFoundException("AccountId does not exist in system");
                }
                
                return _mapper.Map<User>(user);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }

        }

        #endregion
        //ban user
        public async Task<User> BanUserAsync(int accountId)
        {
            try
            {
                User user = await _unitOfWork.UserDAO.GetUserByAccountIdAsync(accountId);
                if (user == null)
                {
                    throw new NotFoundException("AccountId does not exist in system.");
                }
                //user.Accounts. = Convert.ToBoolean((int)AccountEnum.Status.INACTIVE);
                //_unitOfWork.UserDAO.BanUser(user);
                //await this._unitOfWork.CommitAsync();
                //return _mapper.Map<User>(user);

                // Lặp qua tất cả các tài khoản và thiết lập trạng thái
                foreach (var account in user.Accounts)
                {
                    account.IsActive = false; // Đặt IsActive thành false
                }

                _unitOfWork.UserDAO.BanUser(user);
                await this._unitOfWork.CommitAsync();
                return _mapper.Map<User>(user);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new NotFoundException(error);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
    }
}
