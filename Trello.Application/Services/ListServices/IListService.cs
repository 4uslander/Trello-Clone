using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.List;

namespace Trello.Application.Services.ListServices
{
    public interface IListService
    {
        public Task<GetListDetail> CreateListAsync(CreateListDTO requestBody);
        public List<GetListDetail> GetAllList(string? name);
        public Task<GetListDetail> UpdateListAsync(int id, UpdateListDTO requestBody);
        public Task<GetListDetail> ChangeStatusAsync(int Id);
        public Task IsExistListName(string? name);
        public Task IsExistBoardId(int? id);
        Task IsUniqueListPosition(int boardId, int position);
    }
}
