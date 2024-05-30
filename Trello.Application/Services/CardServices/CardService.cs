using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.CardServices
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CardService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CardDetail> CreateCardAsync(CreateCardDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            await IsExistCardTitle(requestBody.Title);
            await IsExistListId(requestBody.ListId);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var card = _mapper.Map<Card>(requestBody);
            card.Id = Guid.NewGuid();
            card.IsActive = true;
            card.ListId = requestBody.ListId;
            card.CreatedDate = DateTime.UtcNow;
            card.CreatedUser = currentUserId;

            await _unitOfWork.CardRepository.InsertAsync(card);
            await _unitOfWork.SaveChangesAsync();

            var createdCardDto = _mapper.Map<CardDetail>(card);
            return createdCardDto;
        }

        public List<CardDetail> GetAllList(string? title)
        {
            IQueryable<Card> cardsQuery = _unitOfWork.CardRepository.GetAll();


            if (!string.IsNullOrEmpty(title))
            {
                cardsQuery = cardsQuery.Where(u => u.Title.Contains(title));
            }

            List<CardDetail> cards = cardsQuery
                .Select(u => _mapper.Map<CardDetail>(u))
                .ToList();

            return cards;
        }
        public async Task<CardDetail> UpdateCardAsync(Guid id, UpdateCardDTO requestBody)
        {
            var card = await _unitOfWork.CardRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);

            await IsExistCardTitle(requestBody.Title);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Validate that EndDate is later than StartDate
            if (requestBody.EndDate.HasValue && requestBody.StartDate.HasValue)
            {
                if (requestBody.EndDate.Value <= requestBody.StartDate.Value)
                {
                    throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.DATE_FIELD, ErrorMessage.INVALID_END_DATE);
                }
            }
            // Validate that ReminderDate is equal or later than EndDate
            if (requestBody.ReminderDate.HasValue && requestBody.EndDate.HasValue)
            {
                if (requestBody.ReminderDate.Value < requestBody.EndDate.Value)
                {
                    throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.DATE_FIELD, ErrorMessage.INVALID_REMINDER_DATE);
                }
            }

            card = _mapper.Map(requestBody, card);

            card.UpdatedDate = DateTime.UtcNow;
            card.UpdatedUser = currentUserId;

            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            var cardDetail = _mapper.Map<CardDetail>(card);
            return cardDetail;
        }

        public async Task<CardDetail> ChangeStatusAsync(Guid Id)
        {
            var card = await _unitOfWork.CardRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            card.UpdatedDate = DateTime.Now;
            card.UpdatedUser = currentUserId;

            if (card.IsActive == true)
            {
                card.IsActive = false;
            }
            else
            {
                card.IsActive = true;
            }

            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<CardDetail>(card);
            return mappedList;
        }

        public async System.Threading.Tasks.Task IsExistCardTitle(string? title)
        {
            var isExist = await _unitOfWork.CardRepository.AnyAsync(x => x.Title.Equals(title));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TITLE_FIELD, ErrorMessage.TITLE_ALREADY_EXIST);
        }

        public async System.Threading.Tasks.Task IsExistListId(Guid? id)
        {
            var listExists = await _unitOfWork.ListRepository.AnyAsync(x => x.Id.Equals(id));
            if (!listExists)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
        }
    }
}
