using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Comment;

namespace Trello.Application.Utilities.Helper.SignalRHub
{
    public class CommentHub : Hub
    {
        public async Task SendComment(CommentDetail comment)
        {
            await Clients.All.SendAsync("ReceiveComment", comment);
        }

        public async Task UpdateComment(CommentDetail comment)
        {
            await Clients.All.SendAsync("UpdateComment", comment);
        }

        public async Task DeleteComment(Guid commentId)
        {
            await Clients.All.SendAsync("DeleteComment", commentId);
        }
    }
}
