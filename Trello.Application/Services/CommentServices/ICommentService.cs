using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Comment;
using Trello.Application.DTOs.List;

namespace Trello.Application.Services.CommentServices
{
    public interface ICommentService
    {
        public Task<CommentDetail> CreateCommentAsync(CommentDTO requestBody);
    }
}
