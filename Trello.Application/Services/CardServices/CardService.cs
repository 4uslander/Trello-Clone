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
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Verify that the list exists
            var existingList = await _listService.GetListByIdAsync(requestBody.ListId);
            if (existingList == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
            }

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the request body to a Card entity and set metadata
            var card = _mapper.Map<Card>(requestBody);
            card.Id = Guid.NewGuid();
            card.IsActive = true;
            card.ListId = requestBody.ListId;
            card.CreatedDate = DateTime.UtcNow;
            card.CreatedUser = currentUserId;

            // Insert the new card into the repository
            await _unitOfWork.CardRepository.InsertAsync(card);
            await _unitOfWork.SaveChangesAsync();

            // Map the created card to a CardDetail DTO
            var createdCardDto = _mapper.Map<CardDetail>(card);
            return createdCardDto;
        }

        public async Task<List<CardDetail>> GetAllCardAsync(Guid listId)
        {
            // Query to get all active cards for a specific list
            IQueryable<Card> cardsQuery = _unitOfWork.CardRepository.GetAll();
            cardsQuery = cardsQuery.Where(u => u.ListId == listId && u.IsActive);

            // Map the cards to CardDetail DTOs
            List<CardDetail> cards = await cardsQuery
                .Select(u => _mapper.Map<CardDetail>(u))
                .ToListAsync();
            return cards;
        }

        public async Task<List<CardDetail>> GetCardByFilterAsync(Guid listId, string? title, bool? isActive)
        {
            // Query to get all cards for a specific list
            IQueryable<Card> cardsQuery = _unitOfWork.CardRepository.GetAll();
            cardsQuery = cardsQuery.Where(c => c.ListId == listId);

            // Apply filters based on the provided parameters
            if (!string.IsNullOrEmpty(title))
            {
                cardsQuery = cardsQuery.Where(c => c.Title.Contains(title));
            }

            if (isActive.HasValue)
            {
                cardsQuery = cardsQuery.Where(c => c.IsActive == isActive.Value);
            }

            // Map the filtered cards to CardDetail DTOs
            List<CardDetail> cards = await cardsQuery
                .Select(c => _mapper.Map<CardDetail>(c))
                .ToListAsync();

            return cards;
        }

        public async Task<CardDetail> UpdateCardAsync(Guid id, UpdateCardDTO requestBody)
        {
            // Get the card by ID and throw an exception if it doesn't exist
            var card = await _unitOfWork.CardRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);

            // Get the current user ID
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

            // Map the request body to the card entity and set metadata
            card = _mapper.Map(requestBody, card);
            card.UpdatedDate = DateTime.UtcNow;
            card.UpdatedUser = currentUserId;
            card.StartDate = requestBody.StartDate;
            card.EndDate = requestBody.EndDate;
            card.ReminderDate = requestBody.ReminderDate;

            // Update the card in the repository
            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated card to a CardDetail DTO
            var cardDetail = _mapper.Map<CardDetail>(card);
            return cardDetail;
        }

        public async Task<CardDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            // Get the card by ID and throw an exception if it doesn't exist
            var card = await _unitOfWork.CardRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the card's status and metadata
            card.UpdatedDate = DateTime.UtcNow;
            card.UpdatedUser = currentUserId;
            card.IsActive = isActive;

            // Update the card in the repository
            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated card to a CardDetail DTO
            var mappedList = _mapper.Map<CardDetail>(card);
            return mappedList;
        }

        public async Task<CardDetail> MoveCardAsync(Guid cardId, Guid newListId)
        {
            // Get the card by ID and throw an exception if it doesn't exist
            var card = await _unitOfWork.CardRepository.GetByIdAsync(cardId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);

            // Verify that the new list exists
            var newList = await _listService.GetListByIdAsync(newListId);
            if (newList == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
            }

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the card's list ID and metadata
            card.ListId = newListId;
            card.UpdatedDate = DateTime.UtcNow;
            card.UpdatedUser = currentUserId;

            // Update the card in the repository
            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated card to a CardDetail DTO
            var cardDetail = _mapper.Map<CardDetail>(card);
            return cardDetail;
        }

        public async Task<Card> GetCardByIdAsync(Guid cardId)
        {
            // Get the card by ID
            return await _unitOfWork.CardRepository.GetByIdAsync(cardId);
        }

    }
}
