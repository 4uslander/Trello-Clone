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
using Trello.Application.Utilities.Helper.SignalRHub.UserConnection;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace Trello.Application.Utilities.Helper.SignalRHub
{
    public class SignalHub : Hub
    {
        private readonly INotificationService _notificationService;
        private readonly IUserConnectionManager _userConnectionManager;

        public SignalHub(INotificationService notificationService, IUserConnectionManager userConnectionManager)
        {
            _notificationService = notificationService;
            _userConnectionManager = userConnectionManager;
        }

        public async Task SendComment(CommentDetail comment)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveComment.ToString(), comment);
        }

        public async Task UpdateComment(CommentDetail comment)
        {
            await Clients.All.SendAsync(SignalRHubEnum.UpdateComment.ToString(), comment);
        }

        public async Task<int> GetTotalNotification(Guid userId)
        {
            var notifications = await _notificationService.GetAllNotificationAsync(userId);
            var totalCount = notifications.TotalCount;
            await Clients.Caller.SendAsync(SignalRHubEnum.ReceiveTotalNotification.ToString(), totalCount);
            return totalCount;
        }

        public async Task SendActivity(CardActivityDetail activity)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveActivity.ToString(), activity);
        }

        public async Task SendNotification(NotificationDetail notification)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveNotification.ToString(), notification);
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.Features.Get<IHttpContextFeature>()?.HttpContext;
            var userId = httpContext?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.Features.Get<IHttpContextFeature>()?.HttpContext;
            var userId = httpContext?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnectionManager.RemoveUserConnection(userId, Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
