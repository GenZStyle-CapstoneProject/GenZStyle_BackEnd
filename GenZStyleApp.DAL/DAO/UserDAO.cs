using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static object instanceLook = new object();

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLook)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }

                    return instance;
                }
            }
        }

        private readonly GenZStyleDbContext _context = new GenZStyleDbContext();

        public List<User> GetAllUser()
        {
            return _context.Users
                .Include(f => f.Role)
                /*.Where(o => o.CarStatus == 1)*/
                .ToList();
        }

        public void AddUser(User user)
        {
            /*var maxId = _context.CarInformations.Max(c => c.CarId);
            car.CarId = maxId + 1;*/

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUserByid(int id)
        {
            var user = _context.Users.Where(c => c.UserId == id).ToList();
            if (user.Count == 0)
            {
                return null;
            }
            else
            {
                return user[0];
            }
        }

        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.UserId);
            if (existingUser != null)
            {
                user.Role = null;
                

                _context.Entry(existingUser).State = EntityState.Detached;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
        public void DeleteUser(int id)
        {
            var user = _context.Users.Where(c => c.UserId == id).ToList()[0];
            var isCarInRenting = _context.Accounts.Where(c => c.UserId == id).ToList().Count > 0;
            if (isCarInRenting) // have in rent --> Update status
            {
                //user.CarStatus = 0;
                user.Role = null;
                

                _context.Users.Update(user);
                _context.SaveChanges();
            }
            else // Not have in rent --> Remove
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
