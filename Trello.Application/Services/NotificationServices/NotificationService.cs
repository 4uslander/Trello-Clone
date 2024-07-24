using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<NotificationDetail> CreateNotificationAsync(NotificationDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var notification = _mapper.Map<Notification>(requestBody);
            notification.Id = Guid.NewGuid();
            notification.CreatedDate = DateTime.UtcNow;
            notification.IsRead = false;
            notification.Title = requestBody.Title;
            notification.Body = requestBody.Body;

            await _unitOfWork.NotificationRepository.InsertAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            var createdNotificationDto = _mapper.Map<NotificationDetail>(notification);
            return createdNotificationDto;
        }

        public async Task<List<NotificationDetail>> GetAllNotificationAsync(Guid userId)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            IQueryable<Notification> notificationsQuery = _unitOfWork.NotificationRepository.GetAll();

            notificationsQuery = notificationsQuery.Where(u => u.UserId == userId && !u.IsRead);

            List<NotificationDetail> notifications = await notificationsQuery
                .Select(u => _mapper.Map<NotificationDetail>(u))
                .ToListAsync();

            return notifications;
        }

        public async Task<List<NotificationDetail>> GetNotificationByFilterAsync(Guid userId, string? title, bool? isRead)
        {
            IQueryable<Notification> notificationsQuery = _unitOfWork.NotificationRepository.GetAll();

            notificationsQuery = notificationsQuery.Where(u => u.UserId == userId);

            if (!string.IsNullOrEmpty(title))
            {
                notificationsQuery = notificationsQuery.Where(u => u.Title.Contains(title));
            }
            if (isRead.HasValue)
            {
                notificationsQuery = notificationsQuery.Where(u => u.IsRead == isRead.Value);
            }

            List<NotificationDetail> notifications = await notificationsQuery
                   .Select(u => _mapper.Map<NotificationDetail>(u))
                   .ToListAsync();

            return notifications;
        }
        public async Task<int> GetNotificationCountAsync(Guid userId)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            int notificationCount = await _unitOfWork.NotificationRepository.GetAll()
                .CountAsync(u => u.UserId == userId && !u.IsRead);

            return notificationCount;
        }


        public async Task<NotificationDetail> ChangeStatusAsync(Guid id, bool isRead)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.NOTIFICATION_FIELD, ErrorMessage.NOTIFICATION_NOT_FOUND);

            notification.UpdatedDate = DateTime.UtcNow;
            notification.IsRead = isRead;

            _unitOfWork.NotificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<NotificationDetail>(notification);
            return mappedList;
        }
    }
}
