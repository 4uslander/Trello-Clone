using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.CardLabel;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.LabelServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.CardLabelServices
{
    public class CardLabelService : ICardLabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        private readonly ILabelService _labelService;
        private readonly IBoardService _boardService;

        public CardLabelService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            ICardService cardService, ILabelService labelService, IBoardService boardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _labelService = labelService;
            _boardService = boardService;
        }


        public async Task<CardLabelDetail> CreateCardLabelAsync(CardLabelDTO requestBody)
        {
            if (requestBody == null)
            {
                // Throws an exception if the request body is null
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }

            // Validates if the card exists
            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            // Validates if the label exists
            var existingLabel = await _labelService.GetLabelByIdAsync(requestBody.LabelId);
            if (existingLabel == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LABEL_FIELD, ErrorMessage.LABEL_NOT_EXIST);
            }

            // Gets the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Maps the DTO to the CardLabel entity and sets additional properties
            var cardLabel = _mapper.Map<CardLabel>(requestBody);
            cardLabel.Id = Guid.NewGuid();
            cardLabel.CreatedDate = DateTime.Now;
            cardLabel.CreatedUser = currentUserId;
            cardLabel.IsActive = true;

            // Inserts the card label into the database and saves changes
            await _unitOfWork.CardLabelRepository.InsertAsync(cardLabel);
            await _unitOfWork.SaveChangesAsync();

            // Maps the created card label to a DTO and returns it
            var createCardLabelDto = _mapper.Map<CardLabelDetail>(cardLabel);
            return createCardLabelDto;
        }

        public async Task<List<CardLabelDetail>> GetAllCardLabelAsync(Guid cardId)
        {
            // Builds the query to retrieve active card labels for the specified card ID
            IQueryable<CardLabel> cardLabelQuery = _unitOfWork.CardLabelRepository.GetAll();
            cardLabelQuery = cardLabelQuery.Where(u => u.CardId == cardId && u.IsActive);

            // Maps the entities to DTOs and returns the list
            List<CardLabelDetail> list = await cardLabelQuery
               .Select(u => new CardLabelDetail
               {
                   Id = u.Id,
                   CardId = u.CardId,
                   LabelId = u.LabelId,
                   LabelName = u.Label.Name,
                   LabelColor = u.Label.Color,
                   CreatedDate = u.CreatedDate,
                   CreatedUser = u.CreatedUser,
                   UpdatedDate = u.UpdatedDate,
                   UpdatedUser = u.UpdatedUser,
                   IsActive = u.IsActive
               }).ToListAsync();

            return list;
        }

        public async Task<List<CardLabelDetail>> GetCardLabelByFilterAsync(Guid cardId, string? labelName, bool? isActive)
        {
            // Builds the query to retrieve card labels for the specified card ID
            IQueryable<CardLabel> cardLabelQuery = _unitOfWork.CardLabelRepository.GetAll();
            cardLabelQuery = cardLabelQuery.Where(u => u.CardId == cardId);

            // Applies filter by label name if provided
            if (!string.IsNullOrEmpty(labelName))
            {
                cardLabelQuery = cardLabelQuery.Where(u => u.Label.Name.Contains(labelName));
            }

            // Applies filter by active status if provided
            if (isActive.HasValue)
            {
                cardLabelQuery = cardLabelQuery.Where(u => u.IsActive == isActive.Value);
            }

            // Maps the entities to DTOs and returns the filtered list
            List<CardLabelDetail> list = await cardLabelQuery
               .Select(u => new CardLabelDetail
               {
                   Id = u.Id,
                   CardId = u.CardId,
                   LabelId = u.LabelId,
                   LabelName = u.Label.Name,
                   LabelColor = u.Label.Color,
                   CreatedDate = u.CreatedDate,
                   CreatedUser = u.CreatedUser,
                   UpdatedDate = u.UpdatedDate,
                   UpdatedUser = u.UpdatedUser,
                   IsActive = u.IsActive
               }).ToListAsync();

            return list;
        }

        public async Task<CardLabelDetail> ChangeStatusCardLabelAsync(Guid Id, bool? isActive)
        {
            // Retrieves the card label by ID or throws an exception if not found
            var cardLabel = await _unitOfWork.CardLabelRepository.GetByIdAsync(Id)
                 ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_LABEL_FIELD, ErrorMessage.CARD_LABEL_NOT_EXIST);

            // Gets the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Updates the card label's status and metadata
            cardLabel.UpdatedDate = DateTime.UtcNow;
            cardLabel.UpdatedUser = currentUserId;
            cardLabel.IsActive = isActive.Value;

            // Updates the card label in the database and saves changes
            _unitOfWork.CardLabelRepository.Update(cardLabel);
            await _unitOfWork.SaveChangesAsync();

            // Maps the updated card label to a DTO and returns it
            var mappedCardLabel = _mapper.Map<CardLabelDetail>(cardLabel);
            return mappedCardLabel;
        }

        public async Task<CardLabel> GetCardLabelByLabelIdAsync(Guid cardId, Guid labelId)
        {
            // Retrieves the card label by card ID and label ID
            return await _unitOfWork.CardLabelRepository.FirstOrDefaultAsync(x => x.LabelId.Equals(labelId) && x.CardId == cardId);
        }

        public async Task<CardLabelDetail> UpdateCardLabelAsync(Guid id, Guid labelId)
        {
            // Retrieves the card label by ID or throws an exception if not found
            var cardLabel = await _unitOfWork.CardLabelRepository.GetByIdAsync(id)
                 ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_LABEL_FIELD, ErrorMessage.CARD_LABEL_NOT_EXIST);

            // Gets the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Updates the label ID and metadata for the card label
            cardLabel.LabelId = labelId;
            cardLabel.UpdatedDate = DateTime.UtcNow;
            cardLabel.UpdatedUser = currentUserId;

            // Updates the card label in the database and saves changes
            _unitOfWork.CardLabelRepository.Update(cardLabel);
            await _unitOfWork.SaveChangesAsync();

            // Maps the updated card label to a DTO and returns it
            var cardLabelDetail = _mapper.Map<CardLabelDetail>(cardLabel);
            return cardLabelDetail;
        }
    }
}
