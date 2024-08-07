﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.BoardMember;
using Trello.Domain.Models;

namespace Trello.Application.Services.BoardMemberServices
{
    public interface IBoardMemberService
    {
        public Task<BoardMemberDetail> CreateBoardMemberAsync(BoardMemberDTO requestBody);
        public Task<List<BoardMemberDetail>> GetAllBoardMemberAsync(Guid boardId);
        public Task<List<BoardMemberDetail>> GetBoardMemberByFilterAsync(Guid boardId, string? name, bool? isActive);
        public Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, Guid roleId);
        public Task<BoardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<BoardMember> GetBoardMemberByUserIdAsync(Guid userId);
        public Task<string> GetCurrentRoleAsync(Guid boardId);
    }
}
