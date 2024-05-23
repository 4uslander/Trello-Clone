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
        public Task<ListDetail> CreateListAsync(ListDTO requestBody);
        public List<ListDetail> GetAllList(string? name);
        public Task<ListDetail> UpdateListAsync(Guid id, UpdateListDTO requestBody);
        public Task<ListDetail> ChangeStatusAsync(Guid Id);
        public Task IsExistListName(string? name, Guid boardId);
        public Task IsExistBoardId(Guid? id);
    }
}
