using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository
{
    public interface IUserRepository
    {
        public void CreateUser(User newUser);
        public void DeleteUserByid(int id);
        public List<User> GetAllUser();
        public User GetUserById(int id);
        public void UpdateUser(User newUser);
    }
}
