using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.ToDo;

namespace Trello.Application.Services.ToDoServices
{
    public interface IToDoService
    {
        public Task<ToDoDetail> CreateToDoListAsync(ToDoDTO requestBody);
        public Task<List<ToDoDetail>> GetAllToDoListAsync(Guid cardId, string? title);
        public Task<ToDoDetail> UpdateToDoListAsync(Guid id, string title);
        public Task<ToDoDetail> ChangeStatusAsync(Guid Id, bool isActive);
    }
}
