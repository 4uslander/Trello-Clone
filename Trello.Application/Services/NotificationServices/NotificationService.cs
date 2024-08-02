using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Notification;
using Trello.Application.DTOs.ToDo;
using Trello.Application.DTOs.User;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Application.Utilities.Helper.SignalRHub;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IUserService userService, IHubContext<SignalHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _hubContext = hubContext;
        }

        public async Task<NotificationDetail> CreateNotificationAsync(NotificationDTO requestBody)
        {
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Check if the specified user exists
            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            // Map the request body to a Notification entity and set its properties
            var notification = _mapper.Map<Notification>(requestBody);
            notification.Id = Guid.NewGuid();
            notification.CreatedDate = DateTime.UtcNow;
            notification.IsRead = false;
            notification.Title = requestBody.Title;
            notification.Body = requestBody.Body;

            // Insert the new notification into the repository and save changes
            await _unitOfWork.NotificationRepository.InsertAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            // Map the created notification to a NotificationDetail DTO and return it
            var createdNotificationDto = _mapper.Map<NotificationDetail>(notification);

            // Get the total number of notifications for the user
            var totalNotifications = await GetNotificationCountAsync(requestBody.UserId);

            // Send the total number of notifications to the client via SignalR
            await _hubContext.Clients.All.SendAsync(SignalRHubEnum.ReceiveTotalNotification.ToString(), totalNotifications);

            return createdNotificationDto;
        }

        public async Task<(List<NotificationDetail> Notifications, int TotalCount)> GetAllNotificationAsync(Guid userId)
        {
            // Check if the specified user exists
            var existingUser = await _userService.GetUserByIdAsync(userId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            // Get all unread notifications for the specified user
            IQueryable<Notification> notificationsQuery = _unitOfWork.NotificationRepository.GetAll();
            notificationsQuery = notificationsQuery.Where(u => u.UserId == userId && !u.IsRead);

            // Get the total count of unread notifications
            int totalCount = await notificationsQuery.CountAsync();

            // Map the notifications to NotificationDetail DTOs and return them
            List<NotificationDetail> notifications = await notificationsQuery
                .OrderByDescending(u => u.CreatedDate)
                .Select(u => _mapper.Map<NotificationDetail>(u))
                .ToListAsync();

            return (notifications, totalCount);
        }

        public async Task<List<NotificationDetail>> GetNotificationByFilterAsync(Guid userId, string? title, bool? isRead)
        {
            // Get all notifications for the specified user
            IQueryable<Notification> notificationsQuery = _unitOfWork.NotificationRepository.GetAll();
            notificationsQuery = notificationsQuery.Where(u => u.UserId == userId);

            // Filter notifications by title if provided
            if (!string.IsNullOrEmpty(title))
            {
                notificationsQuery = notificationsQuery.Where(u => u.Title.Contains(title));
            }

            // Filter notifications by read status if provided
            if (isRead.HasValue)
            {
                notificationsQuery = notificationsQuery.Where(u => u.IsRead == isRead.Value);
            }

            // Map the notifications to NotificationDetail DTOs and return them
            List<NotificationDetail> notifications = await notificationsQuery
                   .Select(u => _mapper.Map<NotificationDetail>(u))
                   .ToListAsync();

            return notifications;
        }

        public async Task<int> GetNotificationCountAsync(Guid userId)
        {
            // Check if the specified user exists
            var existingUser = await _userService.GetUserByIdAsync(userId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            // Get the count of unread notifications for the user
            int notificationCount = await _unitOfWork.NotificationRepository.GetAll()
                .CountAsync(u => u.UserId == userId && !u.IsRead);

            return notificationCount;
        }

        public async Task<NotificationDetail> ChangeStatusAsync(Guid id, bool isRead)
        {
            // Get the notification by ID and throw an exception if it doesn't exist
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.NOTIFICATION_FIELD, ErrorMessage.NOTIFICATION_NOT_FOUND);

            // Update the status and metadata of the notification
            notification.UpdatedDate = DateTime.UtcNow;
            notification.IsRead = isRead;

            // Update the notification in the repository and save changes
            _unitOfWork.NotificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated notification to NotificationDetail and return it
            var mappedList = _mapper.Map<NotificationDetail>(notification);
            return mappedList;
        }

    }
}
