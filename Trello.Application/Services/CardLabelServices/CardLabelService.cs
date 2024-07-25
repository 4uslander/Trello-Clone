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
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }

            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            var existingLabel = await _labelService.GetLabelByIdAsync(requestBody.LabelId);
            if(existingLabel == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LABEL_FIELD, ErrorMessage.LABEL_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var cardLabel = _mapper.Map<CardLabel>(requestBody);
            cardLabel.Id = Guid.NewGuid();
            cardLabel.CreatedDate = DateTime.Now;
            cardLabel.CreatedUser = currentUserId;
            cardLabel.IsActive = true;

            await _unitOfWork.CardLabelRepository.InsertAsync(cardLabel);
            await _unitOfWork.SaveChangesAsync();

            var createCardLabelDto = _mapper.Map<CardLabelDetail>(cardLabel);
            return createCardLabelDto;
        }


        public async Task<List<CardLabelDetail>> GetAllCardLabelAsync(Guid cardId)
        {
            IQueryable<CardLabel> cardLabelQuery = _unitOfWork.CardLabelRepository.GetAll();
            cardLabelQuery = cardLabelQuery.Where(u => u.CardId == cardId && u.IsActive);
            //List<CardLabelDetail> list = await cardLabelQuery.Select(u => _mapper.Map<CardLabelDetail>(u)).ToListAsync();
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
               }
               ).ToListAsync();
            return list;
        }


        public async Task<List<CardLabelDetail>> GetCardLabelByFilterAsync(Guid cardId, string? labelName, bool? isActive)
        {
            IQueryable<CardLabel> cardLabelQuery = _unitOfWork.CardLabelRepository.GetAll();
            cardLabelQuery = cardLabelQuery.Where(u => u.CardId == cardId);

            if (!string.IsNullOrEmpty(labelName))
            {
                cardLabelQuery = cardLabelQuery.Where(u => u.Label.Name.Contains(labelName));
            }
            if (isActive.HasValue)
            {
                cardLabelQuery = cardLabelQuery.Where(u => u.IsActive == isActive.Value);
            }
            // List<CardLabelDetail> list = await cardLabelQuery.Select(u => _mapper.Map<CardLabelDetail>(u)).ToListAsync();
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
               }
               ).ToListAsync();

            return list;
        }



        public async Task<CardLabelDetail> ChangeStatusCardLabelAsync(Guid Id, bool? isActive)
        {
            var cardLabel = await _unitOfWork.CardLabelRepository.GetByIdAsync(Id)
                 ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_LABEL_FIELD, ErrorMessage.CARD_LABEL_NOT_EXIST);
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            
            cardLabel.UpdatedDate = DateTime.UtcNow;
            cardLabel.UpdatedUser = currentUserId;
            cardLabel.IsActive = isActive.Value;

            _unitOfWork.CardLabelRepository.Update(cardLabel);
            await _unitOfWork.SaveChangesAsync();

            var mappedCardLabel = _mapper.Map<CardLabelDetail>(cardLabel);
            return mappedCardLabel;

        }

        public async Task<CardLabel> GetCardLabelByLabelIdAsync(Guid cardId, Guid labelId)
        {
            return await _unitOfWork.CardLabelRepository.FirstOrDefaultAsync(x => x.LabelId.Equals(labelId) && x.CardId == cardId);
        }

        public async Task<CardLabelDetail> UpdateCardLabelAsync(Guid id, UpdateCardLabelDTO updateCardLabelDTO)
        {
            var cardLabel = await _unitOfWork.CardLabelRepository.GetByIdAsync(id)
                 ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_LABEL_FIELD, ErrorMessage.CARD_LABEL_NOT_EXIST);
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            cardLabel.UpdatedDate = DateTime.UtcNow;
            cardLabel.UpdatedUser = currentUserId;

            _unitOfWork.CardLabelRepository.Update(cardLabel);
            await _unitOfWork.SaveChangesAsync();

            var cardLabelDetail = new CardLabelDetail
            {
                Id = cardLabel.Id,
                CardId = cardLabel.CardId,
                LabelId = cardLabel.LabelId,
                LabelName = cardLabel.Label?.Name,
                LabelColor = updateCardLabelDTO?.Color,
                CreatedDate = cardLabel.CreatedDate,
                CreatedUser = cardLabel.CreatedUser,
                UpdatedDate = cardLabel.UpdatedDate,
                UpdatedUser = cardLabel.UpdatedUser,
                IsActive = cardLabel.IsActive
            };

            Console.WriteLine("cardLabelDetail" + cardLabelDetail);

            return cardLabelDetail;
        }
    }
}
