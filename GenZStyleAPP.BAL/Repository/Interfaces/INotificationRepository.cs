using GenZStyleAPP.BAL.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface INotificationRepository
    {
        public Task<List<GetNotificationResponse>> GetNotificationsAsync();
    }
}
