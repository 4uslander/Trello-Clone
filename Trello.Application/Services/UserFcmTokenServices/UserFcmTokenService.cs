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
using Trello.Application.DTOs.UserFcmToken;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.ToDoServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.UserFcmTokenServices
{
    public class UserFcmTokenService : IUserFcmTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserFcmTokenService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<UserFcmTokenDetail> CreateUserFcmTokenAsync(CreateUserFcmTokenDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var userFcmToken = _mapper.Map<UserFcmToken>(requestBody);
            userFcmToken.Id = Guid.NewGuid();
            userFcmToken.UserId = requestBody.UserId;
            userFcmToken.FcmToken = requestBody.FcmToken;
            userFcmToken.CreatedDate = DateTime.UtcNow;
            userFcmToken.IsActive = true;

            await _unitOfWork.UserFcmTokenRepository.InsertAsync(userFcmToken);
            await _unitOfWork.SaveChangesAsync();

            var createdUserFcmTokenDto = _mapper.Map<UserFcmTokenDetail>(userFcmToken);
            return createdUserFcmTokenDto;
        }

        public async Task<List<UserFcmTokenDetail>> GetAllUserFcmTokenAsync(Guid userId)
        {
            IQueryable<UserFcmToken> userFcmTokensQuery = _unitOfWork.UserFcmTokenRepository.GetAll();

            userFcmTokensQuery = userFcmTokensQuery.Where(u => u.UserId == userId && u.IsActive);

            List<UserFcmTokenDetail> userFcmTokens = await userFcmTokensQuery
                .OrderBy(u => u.CreatedDate)
                .Select(u => _mapper.Map<UserFcmTokenDetail>(u))
                .ToListAsync();

            return userFcmTokens;
        }

        public async Task<UserFcmTokenDetail> UpdateUserFcmTokenAsync(Guid id, UserFcmTokenDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var userFcmToken = await _unitOfWork.UserFcmTokenRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FCM_TOKEN_FIELD, ErrorMessage.USER_FCM_TOKEN_NOT_EXIST);

            userFcmToken.UpdatedDate = DateTime.UtcNow;
            userFcmToken.FcmToken = requestBody.FcmToken;

            _unitOfWork.UserFcmTokenRepository.Update(userFcmToken);
            await _unitOfWork.SaveChangesAsync();

            var userFcmTokenDetail = _mapper.Map<UserFcmTokenDetail>(userFcmToken);
            return userFcmTokenDetail;
        }

        public async Task<UserFcmTokenDetail> ChangeStatusAsync(string fcmToken, Guid userId, bool isActive)
        {
            var id = await GetIdByFcmTokenAndUserIdAsync(fcmToken, userId);

            var userFcmToken = await _unitOfWork.UserFcmTokenRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FCM_TOKEN_FIELD, ErrorMessage.USER_FCM_TOKEN_NOT_EXIST);

            userFcmToken.UpdatedDate = DateTime.UtcNow;
            userFcmToken.IsActive = isActive;

            _unitOfWork.UserFcmTokenRepository.Update(userFcmToken);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<UserFcmTokenDetail>(userFcmToken);
            return mappedList;
        }

        public async Task<Guid> GetIdByFcmTokenAndUserIdAsync(string fcmToken, Guid userId)
        {
            var userFcmToken = await _unitOfWork.UserFcmTokenRepository
                .FirstOrDefaultAsync(uft => uft.FcmToken == fcmToken && uft.UserId == userId && uft.IsActive);

            if (userFcmToken == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FCM_TOKEN_FIELD, ErrorMessage.USER_FCM_TOKEN_NOT_EXIST);
            }

            return userFcmToken.Id;
        }
    }
}
