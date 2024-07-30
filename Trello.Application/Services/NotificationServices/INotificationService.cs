using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Notification;

namespace Trello.Application.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task<NotificationDetail> CreateNotificationAsync(NotificationDTO requestBody);
        public Task<(List<NotificationDetail> Notifications, int TotalCount)> GetAllNotificationAsync(Guid userId);
        public Task<List<NotificationDetail>> GetNotificationByFilterAsync(Guid userId, string? title, bool? isRead);
        public Task<int> GetNotificationCountAsync(Guid userId);
        public Task<NotificationDetail> ChangeStatusAsync(Guid id, bool isRead);
    }
}
