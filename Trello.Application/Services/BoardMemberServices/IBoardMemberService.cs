﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;

namespace Trello.Application.Services.BoardMemberServices
{
    public interface IBoardMemberService
    {
        public Task<BoardMemberDetail> CreateBoardMemberAsync(BoardMemberDTO requestBody);
        public List<BoardMemberDetail> GetAllBoardMember(string? name);
        public Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, BoardMemberDTO requestBody);
        public Task IsExistBoard(Guid boardId);
        public Task IsExistUser(Guid userId);
    }
}