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
using System.Collections;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

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
                    RoleId = 3,
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

        public void DeleteUserByid(int id)
        {
            _unitOfWork.UserDAO.DeleteUser(id);
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

        public async Task<User> GetUserDetailByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.UserDAO.GetUserByIdAsync(id);
                if (product == null)
                {
                    throw new NotFoundException("User id does not exist in the system.");
                }
                var userDTO = _mapper.Map<User>(product);
                //Staff staff = await this._unitOfWork.StaffDAO.GetStaffDetailAsync(product.ModifiedBy);
                //userDTO.Role = staff.FullName;
                return userDTO;
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

        #region UpdateUerAsync
        public async Task<User> UpdateUserAsync(int userId, UpdateUserRequest updateUserRequest, HttpContext httpContext)
        {
            try
            {
                User user = await _unitOfWork.UserDAO.GetUserByIdAsync(userId);
                if (user == null)
                {
                    throw new NotFoundException("User Id does not exist in the system.");
                }
                //if (updateProductRequest.Status != (int)ProductEnum.Status.INACTIVE && updateProductRequest.Status != (int)ProductEnum.Status.STOCKING)
                //{
                //    throw new BadRequestException("Status must be 1 or 0.");
                //}
                //var images = new List<ProductImage>();
                //JwtSecurityToken jwtSecurityToken = TokenHelper.ReadToken(httpContext);
                //string emailFromClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                //var accountStaff = await _unitOfWork.AccountDAO.GetAccountByEmail(emailFromClaim);

                user = _mapper.Map(updateUserRequest, user);

                //Inactive
                //if (updateProductRequest.Status == (int)ProductEnum.Status.INACTIVE)
                //{
                //    foreach (var productMeal in product.ProductMeals)
                //    {
                //        productMeal.Meal.Status = (int)MealEnum.Status.INACTIVE;
                //    }
                //}
                //active
                //if (updateProductRequest.Status == (int)ProductEnum.Status.STOCKING)
                //{
                //    double totalAmount = 0d;
                //    product.ProductMeals = product.ProductMeals.OrderBy(x => x.Amount).ToList();
                //    foreach (var productMeal in product.ProductMeals)
                //    {
                //        totalAmount += productMeal.Amount;
                //        if (totalAmount > updateProductRequest.Total)
                //        {
                //            productMeal.Meal.Status = (int)MealEnum.Status.INACTIVE;
                //        }
                //        else
                //        {
                //            productMeal.Meal.Status = (int)MealEnum.Status.STOCKING;
                //        }

                //    }
                //}

                //product.ModifiedBy = accountStaff.ID;
                //product.ModifiedDate = DateTime.Now;

                //#region Upload image to firebase
                //FileHelper.SetCredentials(fireBaseImage);
                //if (updateProductRequest.RemoveProductImages != null && updateProductRequest.RemoveProductImages.Count > 0)
                //{
                //    foreach (var removeProductImage in updateProductRequest.RemoveProductImages)
                //    {
                //        ProductImage removedImage = product.ProductImages.SingleOrDefault(x => x.Source.ToLower().Equals(removeProductImage.ToLower()));
                //        if (removedImage != null)
                //        {
                //            await FileHelper.DeleteImageAsync(removedImage.ImageID, "Product");
                //            product.ProductImages.Remove(removedImage);
                //        }
                //        else
                //        {
                //            throw new BadRequestException($"Remove Image URL: {removeProductImage} does not exist in this product.");
                //        }
                //    }
                //}

                //if (updateProductRequest.NewProductImages != null && updateProductRequest.NewProductImages.Count > 0)
                //{
                //    foreach (var newProductImage in updateProductRequest.NewProductImages)
                //    {
                //        FileStream fileStream = FileHelper.ConvertFormFileToStream(newProductImage);
                //        Tuple<string, string> result = await FileHelper.UploadImage(fileStream, "Product");
                //        var productImage = new ProductImage
                //        {
                //            Source = result.Item1,
                //            ImageID = result.Item2
                //        };
                //        product.ProductImages.Add(productImage);
                //    }
                //}
                #endregion

                _unitOfWork.UserDAO.UpdateUser(user);
                await this._unitOfWork.CommitAsync();
                return this._mapper.Map<User>(user);
            }
            catch (NotFoundException ex)
            {
                string error = ErrorHelper.GetErrorString("User Id", ex.Message);
                throw new NotFoundException(error);
            }
            catch (BadRequestException ex)
            {
                string fieldNameError = "";
                if (ex.Message.ToLower().Contains("status"))
                {
                    fieldNameError = "Status";
                }
                else if (ex.Message.ToLower().Contains("remove image url"))
                {
                    fieldNameError = "RemoveProductImages";
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
