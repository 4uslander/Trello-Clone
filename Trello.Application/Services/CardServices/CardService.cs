using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;
using Trello.Application.DTOs.Card;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Trello.Application.Services.ListServices;
using Trello.Application.Utilities.Helper.ConvertDate;

namespace Trello.Application.Services.CardServices
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IListService _listService;

        public CardService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IListService listService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _listService = listService;
        }
        public async Task<CardDetail> CreateCardAsync(CreateCardDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingList = await _listService.GetListByIdAsync(requestBody.ListId);
            if (existingList == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var card = _mapper.Map<Card>(requestBody);
            card.Id = Guid.NewGuid();
            card.IsActive = true;
            card.ListId = requestBody.ListId;
            card.CreatedDate = DateTime.Now;
            card.CreatedUser = currentUserId;

            await _unitOfWork.CardRepository.InsertAsync(card);
            await _unitOfWork.SaveChangesAsync();

            var createdCardDto = _mapper.Map<CardDetail>(card);
            return createdCardDto;
        }

        public async Task<List<CardDetail>> GetAllCardAsync(Guid listId)
        {
            IQueryable<Card> cardsQuery = _unitOfWork.CardRepository.GetAll();

            cardsQuery = cardsQuery.Where(u => u.ListId == listId && u.IsActive);

            List<CardDetail> cards = await cardsQuery
                .Select(u => _mapper.Map<CardDetail>(u))
                .ToListAsync();

            return cards;
        }

        public async Task<List<CardDetail>> GetCardByFilterAsync(Guid listId, string? title, bool? isActive)
        {
            IQueryable<Card> cardsQuery = _unitOfWork.CardRepository.GetAll();

            cardsQuery = cardsQuery.Where(c => c.ListId == listId);

            if (!string.IsNullOrEmpty(title))
            {
                cardsQuery = cardsQuery.Where(c => c.Title.Contains(title));
            }

            if (isActive.HasValue)
            {
                cardsQuery = cardsQuery.Where(c => c.IsActive == isActive.Value);
            }

            List<CardDetail> cards = await cardsQuery
                .Select(c => _mapper.Map<CardDetail>(c))
                .ToListAsync();

            return cards;
        }

        public async Task<CardDetail> UpdateCardAsync(Guid id, UpdateCardDTO requestBody)
        {
            var card = await _unitOfWork.CardRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);


            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Validate that EndDate is later than StartDate
            if (requestBody.EndDate.HasValue && requestBody.StartDate.HasValue)
            {
                if (requestBody.EndDate.Value <= requestBody.StartDate.Value)
                {
                    throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.DATE_FIELD, ErrorMessage.INVALID_END_DATE);
                }
            }

            // Validate that ReminderDate is equal or sooner than EndDate
            if (requestBody.ReminderDate.HasValue && requestBody.EndDate.HasValue)
            {
                if (requestBody.ReminderDate.Value > requestBody.EndDate.Value)
                {
                    throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.DATE_FIELD, ErrorMessage.INVALID_REMINDER_DATE);
                }
            }

            // Validate that StartDate is sooner than or equal to ReminderDate
            if (requestBody.ReminderDate.HasValue && requestBody.StartDate.HasValue)
            {
                if (requestBody.StartDate.Value > requestBody.ReminderDate.Value)
                {
                    throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.DATE_FIELD, ErrorMessage.INVALID_START_DATE);
                }
            }

            requestBody.StartDate = ConvertDateTime.ConvertToSEA(requestBody.StartDate);
            requestBody.EndDate = ConvertDateTime.ConvertToSEA(requestBody.EndDate);
            requestBody.ReminderDate = ConvertDateTime.ConvertToSEA(requestBody.ReminderDate);

            card = _mapper.Map(requestBody, card);

            card.UpdatedDate = DateTime.Now;
            card.UpdatedUser = currentUserId;

            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            var cardDetail = _mapper.Map<CardDetail>(card);
            return cardDetail;
        }

        public async Task<CardDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            var card = await _unitOfWork.CardRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            card.UpdatedDate = DateTime.Now;
            card.UpdatedUser = currentUserId;
            card.IsActive = isActive;

            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<CardDetail>(card);
            return mappedList;
        }

        public async Task<Card> GetCardByIdAsync(Guid cardId)
        {
            return await _unitOfWork.CardRepository.GetByIdAsync(cardId);
        }
    }
}
