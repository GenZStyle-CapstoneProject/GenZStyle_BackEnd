using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Accounts;
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
using BMOS.BAL.Helpers;
using GenZStyleAPP.BAL.DTOs.UserRelations;
using Microsoft.Extensions.Hosting;
using GenZStyleAPP.BAL.Helpers;
using GenZStyleApp.BAL.Helpers;
using System.Security.Principal;
using GenZStyleAPP.BAL.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
       
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UserRepository( IUnitOfWork unitOfWork, IMapper mapper)
        {
            
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }


        public async Task<GetUserRelationResponse> FollowUser(int UserId,HttpContext httpContext)
        {
            try 
            {
                var accounts = await _unitOfWork.AccountDAO.GetAccountById(UserId);
                if (accounts == null)
                {
                    throw new NotFoundException("AccountId does not exist in system.");
                }
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);
                GetUserRelationResponse getUserRelation = new GetUserRelationResponse();
                var follow = await _unitOfWork.userRelationDAO.GetFollowByPostIdAndAccount(account.AccountId, UserId);
                if (follow != null)
                {
                    follow.FollowerId = account.AccountId;
                    follow.FollowingId = UserId;
                    follow.isFollow = !follow.isFollow;
                    await _unitOfWork.userRelationDAO.ChangeLike(follow);
                }
                else {
                    
                    UserRelation userRelation = new UserRelation
                    {
                        FollowerId = account.AccountId,
                        FollowingId = UserId,
                        isFollow = true,
                        Account = account,                    
                    };

                    if (userRelation.FollowingId == userRelation.FollowerId)
                    {
                        throw new NotFoundException("AccountId has duplicate in system.");
                    }
                    await _unitOfWork.userRelationDAO.AddFollowAsync(userRelation);                   
                    getUserRelation = _mapper.Map<GetUserRelationResponse>(userRelation);
                }
                await _unitOfWork.CommitAsync();
                return getUserRelation;
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
                var customerByUserName = await _unitOfWork.UserDAO.GetUserByUserNameAsync(registerRequest.UserName);
                if (customerByUserName != null)
                {
                    throw new BadRequestException("UserName already exist in the system.");
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
                

                // assign registerRequest to user
                User user = new User
                {
                    City = registerRequest.City,
                    RoleId = 3,
                    Address = registerRequest.Address,
                    Dob = registerRequest.Dob,
                    Gender = registerRequest.Gender,
                    Phone = registerRequest.Phone,
                    Height = registerRequest.Height,
                    Role = role,
                    Accounts = new List<Account> { account },
                };

                
                if(registerRequest.Avatar != null) 
                {
                    // Upload image to firebase
                FileHelper.SetCredentials(fireBaseImage);
                FileStream fileStream = FileHelper.ConvertFormFileToStream(registerRequest.Avatar);
                Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "User");
                user.AvatarUrl = result.Item1; 
                }



                Wallet wallet = new Wallet()
                {
                    Account = account,
                    Balance = 5000000,
                    CreatDate = DateTime.Now,
                    
                };
                await _unitOfWork.WalletDAO.CreateWalletAsync(wallet);               
                await _unitOfWork.AccountDAO.AddAccountAsync(account);
                await _unitOfWork.UserDAO.AddNewUser(user);
                

                
                //Save to Database
                await _unitOfWork.CommitAsync();

                

                return new GetUserResponse
                {
                    Phone = user.Phone,
                    Height = user.Height,
                    UserID = user.UserId,
                    Address = user.Address,
                    AvatarUrl = user.AvatarUrl,
                    Dob = user.Dob,
                    City = user.City,
                    Gender = user.Gender,
                    Accounts = _mapper.Map<List<GetAccountResponse>>(user.Accounts),
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



        public async Task<List<GetUserResponse>> GetUsersAsync()
        {
            try
            {
                List<User> users = await this._unitOfWork.UserDAO.GetAllUser();
                return this._mapper.Map<List<GetUserResponse>>(users);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }

        public async Task<GetUserResponse> GetActiveUser(int userId)
        {
            try
            {
                User user = await this._unitOfWork.UserDAO.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException("User does not exist in the system.");
                }
                return this._mapper.Map<GetUserResponse>(user);
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
        public async Task<GetUserResponse> UpdateUserProfileByAccountIdAsync(int accountId,
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
                
                
                return  this._mapper.Map<GetUserResponse>(user);
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
        public async Task<GetUserResponse> GetUserByAccountIdAsync(int accountId)
        {
            try
            {
                User user = await _unitOfWork.UserDAO.GetUserByAccountIdAsync(accountId);
                if (user == null)
                {
                    throw new NotFoundException("AccountId does not exist in system");
                }
                
                return _mapper.Map<GetUserResponse>(user);
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
        public async Task<GetAccountResponse> BanUserAsync(int accountId)
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
                return _mapper.Map<GetAccountResponse>(user);

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


        public async Task<GetFollowResponse> GetFollowByProfileIdAsync(HttpContext httpContext)
        {
            try 
            {
                JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                var account = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);


                var followers =  await this._unitOfWork.userRelationDAO.GetFollower(account.AccountId);// người này được bao nhiêu người follow 
                var UserRelationn = _mapper.Map<List<GetUserRelationResponse>>(followers);
                var followerAccounts = new List<GetAccountResponse>(); // Danh sách các tài khoản của người theo dõi
                foreach (var follower in UserRelationn)
                {
                    followerAccounts.Add(follower.Account);
                }
                var followings  =  await this._unitOfWork.userRelationDAO.GetFollowing(account.AccountId);//người này đang follow bao nhiêu người
                var UserRelationnn = _mapper.Map<List<GetUserRelationResponse>>(followings);
                var followingAccounts = new List<GetAccountResponse>(); // Danh sách các tài khoản của người theo dõi
                foreach (var following in UserRelationnn)
                {
                    followingAccounts.Add(following.Account);
                }
                GetFollowResponse followResponse = new GetFollowResponse
                {
                    /*Follower = followers.Count,
                    Followering = followings.Count,*/
                    Followers = followerAccounts,
                    Following = followingAccounts
                };

                return followResponse;
            } 
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

    }
}
