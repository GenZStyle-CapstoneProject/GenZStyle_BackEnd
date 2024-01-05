using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IUserRepository
    {
      
        public Task DeleteUserAsync(int id, HttpContext httpContext);
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetActiveUser(int userId);
        public Task<User> UpdateUserProfileByAccountIdAsync(int accountId,
                                                                                     FireBaseImage fireBaseImage,
                                                                                     UpdateUserRequest updateUserRequest);
        public Task<User> BanUserAsync(int accountId);
        public Task<User> GetUserByAccountIdAsync(int accountId);
        public Task<GetUserResponse> Register(FireBaseImage fireBaseImage,RegisterRequest registerRequest);
    }
}
