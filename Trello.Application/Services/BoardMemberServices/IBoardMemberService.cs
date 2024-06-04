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
        public Task<BoardMemberDetail> CreateBoardMemberAsync(BoardMemberDTO requestBody);
        public Task<List<BoardMemberDetail>> GetAllBoardMemberAsync(Guid boardId, string? name);
        public Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, Guid roleId);
        public Task<BoardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<BoardMember> GetBoardMemberByUserIdAsync(Guid userId);
    }
}