using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;

namespace Trello.Application.Services.BoardServices
{
    public interface IBoardService
    {
        public Task<BoardDetail> CreateBoardAsync(CreateBoardDTO requestBody);
        public List<BoardDetail> GetAllBoard(string? name);
        public Task<BoardDetail> UpdateBoardAsync(Guid id, UpdateBoardDTO requestBody);
        public Task<BoardDetail> ChangeStatusAsync(Guid Id);
        public Task IsExistBoardName(string? name);
    }
}
