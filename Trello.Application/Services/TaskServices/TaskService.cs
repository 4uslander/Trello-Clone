using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Trello.Application.DTOs.Notification;
using Trello.Application.DTOs.Task;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.NotificationServices;
using Trello.Application.Services.ToDoServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.FirebaseNoti;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Enums;
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
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly INotificationService _notificationService;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IBoardMemberService boardMemberService, IToDoService todoService, IFirebaseNotificationService firebaseNotificationService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _boardMemberService = boardMemberService;
            _todoService = todoService;
            _firebaseNotificationService = firebaseNotificationService;
            _notificationService = notificationService;
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
            task.CreatedDate = DateTime.UtcNow;
            task.CreatedUser = currentUserId;
            task.IsChecked = false;
            task.DueDate = requestBody.DueDate;

            await _unitOfWork.TaskRepository.InsertAsync(task);
            await _unitOfWork.SaveChangesAsync();

            //
            if (requestBody.AssignedUserId.HasValue)
            {
                var notificationRequest = new NotificationDTO
                {
                    UserId = requestBody.AssignedUserId.Value,
                    Title = NotificationTitleField.ASSIGNED_TO_TASK,
                    Body = $"\n{NotificationBodyField.ASSIGNED_TO_TASK}: {requestBody.Name}."
                };

                var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);

                await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);
            }

            var createdTaskDto = _mapper.Map<TaskDetail>(task);
            return createdTaskDto;
        }

        public async Task<List<TaskDetail>> GetAllTaskAsync(Guid todoId)
        {
            IQueryable<Domain.Models.Task> tasksQuery = _unitOfWork.TaskRepository.GetAll();

            tasksQuery = tasksQuery.Where(u => u.TodoId == todoId && u.IsActive);

            List<TaskDetail> tasks = await tasksQuery
                .OrderBy(u => u.CreatedDate) 
                .Select(u => _mapper.Map<TaskDetail>(u))
                .ToListAsync();

            return tasks;
        }

        public async Task<List<TaskDetail>> GetTaskByFilterAsync(Guid todoId, string? name, bool? isActive)
        {
            IQueryable<Domain.Models.Task> tasksQuery = _unitOfWork.TaskRepository.GetAll();

            tasksQuery = tasksQuery.Where(u => u.TodoId == todoId && u.IsActive);

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

            task.UpdatedDate = DateTime.UtcNow;
            task.UpdatedUser = currentUserId;
            task.Name = requestBody.Name.ToString();
            task.PriorityLevel = requestBody.PriorityLevel.ToString();
            task.Status = requestBody.Status.ToString();
            task.AssignedUserId = requestBody.AssignedUserId;
            task.Description = requestBody.Description;
            task.DueDate = requestBody.DueDate;

            if (task.Status == TaskStatusEnum.Resolved.ToString())
            {
                task.IsChecked = true;
            }
            else
            {
                task.IsChecked = false;
            }

            _unitOfWork.TaskRepository.Update(task);

            //
            if (requestBody.AssignedUserId.HasValue)
            {
                var notificationRequest = new NotificationDTO
                {
                    UserId = requestBody.AssignedUserId.Value,
                    Title = NotificationTitleField.TASK_UPDATED,
                    Body = $"\n{task.Name} {NotificationBodyField.TASK_UPDATED}"
                };

                var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);

                await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);        
            }

            await _unitOfWork.SaveChangesAsync();
           
            var taskDetail = _mapper.Map<TaskDetail>(task);
            return taskDetail;
        }

        public async Task<TaskDetail> CheckTaskAsync(Guid id, bool isChecked)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TASK_FIELD, ErrorMessage.TASK_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            task.UpdatedDate = DateTime.UtcNow;
            task.UpdatedUser = currentUserId;
            task.IsChecked = isChecked;
            task.CompletedDate = isChecked ? DateTime.UtcNow : (DateTime?)null;
            task.Status = isChecked ? TaskStatusEnum.Resolved.ToString() : TaskStatusEnum.InProgress.ToString();

            _unitOfWork.TaskRepository.Update(task);

            var existingAssignedUser = await GetAssignedUserIdByTaskIdAsync(id);
            if (existingAssignedUser.HasValue)
            {
                var notificationRequest = new NotificationDTO
                {
                    UserId = existingAssignedUser.Value,
                    Title = NotificationTitleField.TASK_CHECKED,
                    Body = $"\n{task.Name} {NotificationBodyField.TASK_CHECKED}"
                };

                var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);

                await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);
            }

            await _unitOfWork.SaveChangesAsync();
         
            var taskDetail = _mapper.Map<TaskDetail>(task);
            return taskDetail;
        }

        public async Task<TaskDetail> ChangeStatusAsync(Guid id, bool isActive)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TASK_FIELD, ErrorMessage.TASK_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            task.UpdatedDate = DateTime.UtcNow;
            task.UpdatedUser = currentUserId;
            task.IsActive = isActive;

            _unitOfWork.TaskRepository.Update(task);

            var existingAssignedUser = await GetAssignedUserIdByTaskIdAsync(id);
            if (existingAssignedUser.HasValue)
            {
                var notificationRequest = new NotificationDTO
                {
                    UserId = existingAssignedUser.Value,
                    Title = NotificationTitleField.TASK_REMOVED,
                    Body = $"\n {task.Name} {NotificationBodyField.TASK_REMOVED}"
                };

                var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);

                await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);
            }

            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<TaskDetail>(task);
            return mappedList;
        }

        public async Task<Guid?> GetAssignedUserIdByTaskIdAsync(Guid taskId)
        {
            var userIdQuery = from task in _unitOfWork.TaskRepository.GetAll()
                              where task.Id == taskId && task.IsActive
                              select task.AssignedUserId;

            Guid? assignedUserId = await userIdQuery.FirstOrDefaultAsync();

            return assignedUserId;
        }

    }
}