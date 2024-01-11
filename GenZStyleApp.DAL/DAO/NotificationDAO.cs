using GenZStyleApp.DAL.DBContext;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class NotificationDAO
    {
        private GenZStyleDbContext _dbContext;
        public NotificationDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddNotiAsync(Notification newNoti)
        {
            try
            {
                await this._dbContext.Notifications.AddAsync(newNoti);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
