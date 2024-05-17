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
        public Task<GetBoardDetail> CreateBoardAsync(CreateBoardDTO requestBody);
        public List<GetBoardDetail> GetAllBoard(SearchBoardDTO searchKey);
        public Task IsExistBoardName(string? name);
    }
}
