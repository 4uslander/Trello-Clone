using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Comment;
using Trello.Application.DTOs.Notification;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Trello.Application.Utilities.Helper.SignalRHub
{
    public class SignalHub : Hub
    {
        public async Task SendComment(CommentDetail comment)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveComment.ToString(), comment);
        }

        public async Task UpdateComment(CommentDetail comment)
        {
            await Clients.All.SendAsync(SignalRHubEnum.UpdateComment.ToString(), comment);
        }

        public async Task GetTotalNotification(Guid userId, int count)
        {
            await Clients.User(userId.ToString()).SendAsync(SignalRHubEnum.ReceiveTotalNotification.ToString(), count);
        }
    }
}
