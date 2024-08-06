using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Task;
using Trello.Application.DTOs.ToDo;

namespace Trello.Application.Services.TaskServices
{
    public interface ITaskService
    {
        public Task<TaskDetail> CreateTaskAsync(CreateTaskDTO requestBody);
        public Task<List<TaskDetail>> GetAllTaskAsync(Guid todoId);
        public Task<List<TaskDetail>> GetTaskByFilterAsync(Guid todoId, string? name, bool? isActive);
        public Task<TaskDetail> UpdateTaskAsync(Guid id, TaskDTO requestBody);
        public Task<TaskDetail> CheckTaskAsync(Guid id, bool isChecked);
        public Task<TaskDetail> ChangeStatusAsync(Guid id, bool isActive);
        Task<List<TaskDetail>> GetTasksForReminderAsync(DateTime currentDate);
    }
}
