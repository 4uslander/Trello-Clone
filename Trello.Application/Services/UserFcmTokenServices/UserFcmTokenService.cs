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
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Check if the user exists
            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            // Map the request body to a UserFcmToken entity and set its properties
            var userFcmToken = _mapper.Map<UserFcmToken>(requestBody);
            userFcmToken.Id = Guid.NewGuid();
            userFcmToken.UserId = requestBody.UserId;
            userFcmToken.FcmToken = requestBody.FcmToken;
            userFcmToken.CreatedDate = DateTime.UtcNow;
            userFcmToken.IsActive = true;

            // Insert the new UserFcmToken into the repository and save changes
            await _unitOfWork.UserFcmTokenRepository.InsertAsync(userFcmToken);
            await _unitOfWork.SaveChangesAsync();

            // Map the created UserFcmToken to a UserFcmTokenDetail DTO and return it
            var createdUserFcmTokenDto = _mapper.Map<UserFcmTokenDetail>(userFcmToken);
            return createdUserFcmTokenDto;
        }

        public async Task<List<UserFcmTokenDetail>> GetAllUserFcmTokenAsync(Guid userId)
        {
            // Get all active UserFcmTokens associated with the specified User ID
            IQueryable<UserFcmToken> userFcmTokensQuery = _unitOfWork.UserFcmTokenRepository.GetAll();
            userFcmTokensQuery = userFcmTokensQuery.Where(u => u.UserId == userId && u.IsActive);

            // Map the UserFcmTokens to UserFcmTokenDetail DTOs and return them
            List<UserFcmTokenDetail> userFcmTokens = await userFcmTokensQuery
                .OrderBy(u => u.CreatedDate)
                .Select(u => _mapper.Map<UserFcmTokenDetail>(u))
                .ToListAsync();

            return userFcmTokens;
        }

        public async Task<UserFcmTokenDetail> UpdateUserFcmTokenAsync(Guid id, UserFcmTokenDTO requestBody)
        {
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Get the UserFcmToken by ID and throw an exception if it doesn't exist
            var userFcmToken = await _unitOfWork.UserFcmTokenRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FCM_TOKEN_FIELD, ErrorMessage.USER_FCM_TOKEN_NOT_EXIST);

            // Update the UserFcmToken properties
            userFcmToken.UpdatedDate = DateTime.UtcNow;
            userFcmToken.FcmToken = requestBody.FcmToken;

            // Update the UserFcmToken in the repository and save changes
            _unitOfWork.UserFcmTokenRepository.Update(userFcmToken);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated UserFcmToken to a UserFcmTokenDetail DTO and return it
            var userFcmTokenDetail = _mapper.Map<UserFcmTokenDetail>(userFcmToken);
            return userFcmTokenDetail;
        }

        public async Task<UserFcmTokenDetail> ChangeStatusAsync(string fcmToken, Guid userId, bool isActive)
        {
            // Get the ID of the UserFcmToken by FcmToken and UserId
            var id = await GetIdByFcmTokenAndUserIdAsync(fcmToken, userId);

            // Get the UserFcmToken by ID and throw an exception if it doesn't exist
            var userFcmToken = await _unitOfWork.UserFcmTokenRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FCM_TOKEN_FIELD, ErrorMessage.USER_FCM_TOKEN_NOT_EXIST);

            // Update the UserFcmToken's active status and metadata
            userFcmToken.UpdatedDate = DateTime.UtcNow;
            userFcmToken.IsActive = isActive;

            // Update the UserFcmToken in the repository and save changes
            _unitOfWork.UserFcmTokenRepository.Update(userFcmToken);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated UserFcmToken to a UserFcmTokenDetail DTO and return it
            var mappedList = _mapper.Map<UserFcmTokenDetail>(userFcmToken);
            return mappedList;
        }

        public async Task<Guid> GetIdByFcmTokenAndUserIdAsync(string fcmToken, Guid userId)
        {
            // Get the UserFcmToken by FcmToken and UserId and throw an exception if it doesn't exist
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
