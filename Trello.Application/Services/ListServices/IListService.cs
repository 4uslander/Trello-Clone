﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.List;
using Trello.Domain.Models;

namespace Trello.Application.Services.ListServices
{
    public interface IListService
    {
        public Task<ListDetail> CreateListAsync(ListDTO requestBody);
        public Task<List<ListDetail>> GetAllListAsync(Guid boardId);
        public Task<List<ListDetail>> GetListByFilterAsync(Guid boardId, string? name, bool? isActive);
        public Task<ListDetail> UpdateListNameAsync(Guid id, ListDTO requestBody);
        public Task<ListDetail> SwapListPositionsAsync(Guid id, Guid swappedListId);
        public Task<ListDetail> MoveListAsync(Guid id, int newPosition);
        public Task<ListDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<List> GetListByNameAsync(string name, Guid boardId);
        public Task<List> GetListByIdAsync(Guid id);
        public Task<Board> GetBoardByIdAsync(Guid id);
    }
}
