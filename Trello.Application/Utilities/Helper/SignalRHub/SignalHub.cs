using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardActivity;
using Trello.Application.DTOs.Comment;
using Trello.Domain.Enums;

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

        public async Task SendActivity(CardActivityDetail activity)
        {
            await Clients.All.SendAsync(SignalRHubEnum.ReceiveActivity.ToString(), activity);
        }
    }
}
