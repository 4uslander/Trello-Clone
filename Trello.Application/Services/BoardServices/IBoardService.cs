using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Domain.Models;

namespace Trello.Application.Services.BoardServices
{
    public interface IBoardService
    {
        public Task<BoardDetail> CreateBoardAsync(BoardDTO requestBody);
        public Task<List<BoardDetail>> GetBoardAsync();
        public Task<List<BoardDetail>> GetBoardByFilterAsync(string? name, bool? isPublic, bool? isActive);
        public Task<BoardDetail> UpdateBoardAsync(Guid id, BoardDTO requestBody);
        public Task<BoardDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<BoardDetail> ChangeVisibilityAsync(Guid Id, bool isPublic);
        public Task<Board> GetBoardByNameAsync(string name);
        public Task<Board> GetBoardByIdAsync(Guid id);
        public Task<Board> GetBoardByCardIdAsync(Guid cardId);
    }
}
