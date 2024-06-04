using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.List;
using Trello.Domain.Models;

namespace Trello.Application.Services.ListServices
{
    public interface IListService
    {
        public Task<ListDetail> CreateListAsync(ListDTO requestBody);
        public Task<List<ListDetail>> GetAllListAsync(Guid boardId, string? name);
        public Task<ListDetail> UpdateListNameAsync(Guid id, ListDTO requestBody);
        public Task<ListDetail> SwapListPositionsAsync(Guid firstListId, Guid secondListId);
        public Task<ListDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<List> GetListByNameAsync(string name, Guid boardId);
        public Task<Board> GetBoardByIdAsync(Guid id);
    }
}
