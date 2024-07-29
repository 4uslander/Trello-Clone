using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardActivity;
using Trello.Application.DTOs.Comment;
using Trello.Application.DTOs.Notification;
using Trello.Application.Services.NotificationServices;
using Trello.Application.Services.UserServices;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using Task = System.Threading.Tasks.Task;

namespace Trello.Application.Utilities.Helper.SignalRHub
{
    public class SignalHub : Hub
    {
        private readonly INotificationService _notificationService;

        public SignalHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task SendComment(CommentDetail comment)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveComment.ToString(), comment);
        }

        public async Task UpdateComment(CommentDetail comment)
        {
            await Clients.All.SendAsync(SignalRHubEnum.UpdateComment.ToString(), comment);
        }

        public async Task GetTotalNotification(Guid userId)
        {
            var notificationCount = await _notificationService.GetNotificationCountAsync(userId);
            await Clients.Caller.SendAsync(SignalRHubEnum.ReceiveTotalNotification.ToString(), notificationCount);
        }

        public async Task SendActivity(CardActivityDetail activity)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveActivity.ToString(), activity);
        }
    }
}
