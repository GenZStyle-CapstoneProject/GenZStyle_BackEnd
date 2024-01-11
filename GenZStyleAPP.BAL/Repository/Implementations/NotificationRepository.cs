using AutoMapper;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.Repository.Interfaces;
using ProjectParticipantManagement.BAL.Heplers;
using ProjectParticipantManagement.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenZStyleAPP.BAL.DTOs.Notifications;

namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public NotificationRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }

        #region GetNotifications
        public async Task<List<GetNotificationResponse>> GetNotificationsAsync()
        {
            try
            {
                List<Notification> notifications = await this._unitOfWork.NotificationDAO.GetNotifications();
                return this._mapper.Map<List<GetNotificationResponse>>(notifications);
            }
            catch (Exception ex)
            {
                string error = ErrorHelper.GetErrorString(ex.Message);
                throw new Exception(error);
            }
        }
        #endregion
    }
}
