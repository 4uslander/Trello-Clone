using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardActivity;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
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
        private readonly ICardService _cardService;
        private readonly IUserService _userService;

        public CardActivityService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICardService cardService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _userService = userService;
        }

        public Task<CardActivityDetail> CreateCardActivityAsync(CreateCardActivityDTO requestBody)
        {
            if(requestBody == null)
            {
                 throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var activity = _mapper.Map<CardActivity>(requestBody);
            
        }

        public async Task<List<CardActivityDetail>> GetCardActivityDetailAsync(Guid cardId)
        {
            IQueryable<CardActivity> cardActivitiesQuery = _unitOfWork.CardActivityRepository.GetAll();
            cardActivitiesQuery = cardActivitiesQuery.Where(u => u.CardId == cardId && u.IsActive);

            List<CardActivityDetail> list = await cardActivitiesQuery.Select(u => _mapper.Map<CardActivityDetail>(u)).ToListAsync();
            return list ;
        }


    }
}
