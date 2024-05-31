using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Domain.Models;

namespace Trello.Application.Services.BoardMemberServices
{
    public interface IBoardMemberService
    {
        public Task<BoardMemberDetail> CreateBoardMemberAsync(CreateBoardMemberDTO requestBody);
        public Task<List<BoardMemberDetail>> GetAllBoardMemberAsync(Guid boardId, string? name);
        public Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, BoardMemberDTO requestBody);
        public Task<BoardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<Board> GetBoardById(Guid boardId);
        public Task<User> GetUserById(Guid userId);
    }
}