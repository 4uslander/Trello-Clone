using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.ToDo;
using Trello.Domain.Models;

namespace Trello.Application.Services.ToDoServices
{
    public interface IToDoService
    {
        public Task<ToDoDetail> CreateToDoListAsync(CreateToDoDTO requestBody);
        public Task<List<ToDoDetail>> GetAllToDoListAsync(Guid cardId);
        public Task<List<ToDoDetail>> GetToDoListByFilterAsync(Guid cardId, string? title, bool? isActive);
        public Task<ToDoDetail> UpdateToDoListAsync(Guid id, ToDoDTO requestBody);
        public Task<ToDoDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<ToDo> GetTodoListByIdAsync(Guid todoId);
    }
}
