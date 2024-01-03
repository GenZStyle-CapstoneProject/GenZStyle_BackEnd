using GenZStyleApp.DAL.Models;
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
        public Task<GetUserResponse> Register(RegisterRequest registerRequest);
        public void DeleteUserByid(int id);
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserDetailByIdAsync(int id);
        public Task<User> UpdateUserAsync(int userId, UpdateUserRequest updateUserRequest, HttpContext httpContext);
    }
}
