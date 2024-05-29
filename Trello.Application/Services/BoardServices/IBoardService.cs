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
        Task<List<BoardDetail>> GetAllBoardAsync(string? name);
        public Task<BoardDetail> UpdateBoardAsync(Guid id, BoardDTO requestBody);
        public Task<BoardDetail> ChangeStatusAsync(Guid Id);
        public Task<BoardDetail> ChangeVisibility(Guid Id);
        public System.Threading.Tasks.Task IsExistBoardName(string? name);
        public Task<Board> IsExistBoardId(Guid? id);
    }
}
