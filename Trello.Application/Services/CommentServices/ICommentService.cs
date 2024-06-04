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
        public Task<List<CommentDetail>> GetAllCommentAsync(Guid cardId);
        public Task<CommentDetail> UpdateCommentAsync(Guid id, string content);
        public Task<CommentDetail> ChangeStatusAsync(Guid id, bool isActive);
    }
}
