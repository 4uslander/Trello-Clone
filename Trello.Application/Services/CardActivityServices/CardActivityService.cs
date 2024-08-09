using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardActivity;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardMemberServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Application.Utilities.Helper.SignalRHub;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.CardActivityServices
{
    public class CardActivityService : ICardActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<SignalHub> _hubContext;

        public CardActivityService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHubContext<SignalHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public async Task<CardActivityDetail> CreateCardActivityAsync(CreateCardActivityDTO requestBody)
        {
            if (requestBody == null)
            {
                // Throws an exception if the request body is null
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }

            // Gets the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Maps the DTO to the CardActivity entity and sets additional properties
            var activity = _mapper.Map<CardActivity>(requestBody);
            activity.Id = Guid.NewGuid();
            activity.CreatedDate = DateTime.UtcNow.ToLocalTime();
            activity.CreatedUser = currentUserId;
            activity.IsActive = true;

            // Inserts the activity into the database and saves changes
            await _unitOfWork.CardActivityRepository.InsertAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            // Maps the created activity to a DTO and sends a real-time notification
            var createdCardActivityDto = _mapper.Map<CardActivityDetail>(activity);
            await _hubContext.Clients.All.SendAsync(SignalRHubEnum.ReceiveActivity.ToString(), createdCardActivityDto);

            return createdCardActivityDto;
        }

        public async Task<List<CardActivityDetail>> GetCardActivityDetailAsync(Guid cardId)
        {
            // Builds the query to retrieve active card activities for the specified card ID
            IQueryable<CardActivity> cardActivitiesQuery = _unitOfWork.CardActivityRepository.GetAll();
            cardActivitiesQuery = cardActivitiesQuery.Where(u => u.CardId == cardId && u.IsActive);

            // Maps the entities to DTOs and returns the list
            List<CardActivityDetail> cardActivities = await cardActivitiesQuery.Select(cm => new CardActivityDetail
            {
                Id = cm.Id,
                CardId = cm.CardId,
                UserId = cm.UserId,
                UserName = cm.User.Name,
                Activity = cm.Activity,
                CreatedDate = cm.CreatedDate,
                CreatedUser = cm.CreatedUser,
                UpdatedDate = cm.UpdatedDate,
                UpdatedUser = cm.UpdatedUser,
                IsActive = cm.IsActive,
            }).ToListAsync();

            return cardActivities;
        }
    }
}
