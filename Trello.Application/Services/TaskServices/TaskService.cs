using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Task;
using Trello.Application.DTOs.ToDo;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.ToDoServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.TaskServices
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBoardMemberService _boardMemberService;
        private readonly IToDoService _todoService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBoardMemberService boardMemberService, IToDoService todoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _boardMemberService = boardMemberService;
            _todoService = todoService;
        }

        public async Task<TaskDetail> CreateTaskAsync(CreateTaskDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingTodoList = await _todoService.GetTodoListByIdAsync(requestBody.TodoId);
            if (existingTodoList == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TODO_FIELD, ErrorMessage.TODO_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var existingBoardMember = await _boardMemberService.GetBoardMemberByUserIdAsync(currentUserId);
            if (existingBoardMember == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);
            }

            var task = _mapper.Map<Domain.Models.Task>(requestBody);
            task.Id = Guid.NewGuid();
            task.IsActive = true;
            task.CreatedDate = DateTime.Now;
            task.CreatedUser = currentUserId;
            task.IsChecked = false;

            await _unitOfWork.TaskRepository.InsertAsync(task);
            await _unitOfWork.SaveChangesAsync();

            var createdTaskDto = _mapper.Map<TaskDetail>(task);
            return createdTaskDto;
        }

        public async Task<List<TaskDetail>> GetAllTaskAsync(Guid todoId)
        {
            IQueryable<Domain.Models.Task> tasksQuery = _unitOfWork.TaskRepository.GetAll();

            tasksQuery = tasksQuery.Where(u => u.TodoId == todoId && u.IsActive);

            List<TaskDetail> tasks = await tasksQuery
                .Select(u => _mapper.Map<TaskDetail>(u))
                .ToListAsync();

            return tasks;
        }

        public async Task<List<TaskDetail>> GetTaskByFilterAsync(Guid todoId, string? name, bool? isActive)
        {
            IQueryable<Domain.Models.Task> tasksQuery = _unitOfWork.TaskRepository.GetAll();

            tasksQuery = tasksQuery.Where(u => u.TodoId == todoId);

            if (!string.IsNullOrEmpty(name))
            {
                tasksQuery = tasksQuery.Where(u => u.Name.Contains(name));
            }
            if (isActive.HasValue)
            {
                tasksQuery = tasksQuery.Where(u => u.IsActive == isActive.Value);
            }

            List<TaskDetail> tasks = await tasksQuery
                .Select(u => _mapper.Map<TaskDetail>(u))
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskDetail> UpdateTaskAsync(Guid id, TaskDTO requestBody)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TASK_FIELD, ErrorMessage.TASK_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            task.UpdatedDate = DateTime.Now;
            task.UpdatedUser = currentUserId;
            task.Name = requestBody.Name;

            _unitOfWork.TaskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();

            var taskDetail = _mapper.Map<TaskDetail>(task);
            return taskDetail;
        }

        public async Task<TaskDetail> CheckTaskAsync(Guid id, bool isChecked)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TASK_FIELD, ErrorMessage.TASK_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            task.UpdatedDate = DateTime.Now;
            task.UpdatedUser = currentUserId;
            task.IsChecked = isChecked;

            _unitOfWork.TaskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();

            var taskDetail = _mapper.Map<TaskDetail>(task);
            return taskDetail;
        }

        public async Task<TaskDetail> ChangeStatusAsync(Guid id, bool isActive)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TASK_FIELD, ErrorMessage.TASK_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            task.UpdatedDate = DateTime.Now;
            task.UpdatedUser = currentUserId;
            task.IsActive = isActive;

            _unitOfWork.TaskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<TaskDetail>(task);
            return mappedList;
        }
    }
}