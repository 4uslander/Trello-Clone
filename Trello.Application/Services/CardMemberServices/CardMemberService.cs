using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.CardMember;
using Trello.Application.DTOs.List;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.FirebaseNoti;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.CardMemberServices
{
    public class CardMemberService : ICardMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        private readonly IUserService _userService;
        private readonly IBoardMemberService _boardMemberService;
        private readonly IFirebaseNotificationService _notificationService;

        public CardMemberService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICardService cardService, IUserService userService, IBoardMemberService boardMemberService, IFirebaseNotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _userService = userService;
            _boardMemberService = boardMemberService;
            _notificationService = notificationService;
        }

        public async Task<CardMemberDetail> CreateCardMemberAsync(CardMemberDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var existingBoardMember = await _boardMemberService.GetBoardMemberByUserIdAsync(requestBody.UserId);
            if (existingBoardMember == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var cardMember = _mapper.Map<CardMember>(requestBody);
            cardMember.Id = Guid.NewGuid();
            cardMember.CreatedDate = DateTime.UtcNow;
            cardMember.CreatedUser = currentUserId;
            cardMember.IsActive = true;

            await _unitOfWork.CardMemberRepository.InsertAsync(cardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardMemberDto = _mapper.Map<CardMemberDetail>(cardMember);
           
            //
            await _notificationService.SendNotificationAsync(requestBody.UserId, "You have been invited to a new Card!", $"You have invited added to the card: {existingCard.Title}.");

            return createdBoardMemberDto;
        }

        public async Task<List<CardMemberDetail>> GetAllCardMemberAsync(Guid cardId)
        {
            IQueryable<CardMember> cardsQuery = _unitOfWork.CardMemberRepository.GetAll();

            cardsQuery = cardsQuery.Where(u => u.CardId == cardId && u.IsActive);

            List<CardMemberDetail> lists = await cardsQuery
                .Select(u => _mapper.Map<CardMemberDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<List<CardMemberDetail>> GetCardMemberByFilterAsync(Guid cardId, string? userName, bool? isActive)
        {
            IQueryable<CardMember> cardsQuery = _unitOfWork.CardMemberRepository.GetAll();

            cardsQuery = cardsQuery.Where(cm => cm.CardId == cardId);

            if (!string.IsNullOrEmpty(userName))
            {
                cardsQuery = cardsQuery.Where(cm => cm.User.Name.Contains(userName));
            }

            if (isActive.HasValue)
            {
                cardsQuery = cardsQuery.Where(cm => cm.IsActive == isActive.Value);
            }

            List<CardMemberDetail> lists = await cardsQuery
                .Select(cm => _mapper.Map<CardMemberDetail>(cm))
                .ToListAsync();

            return lists;
        }

        public async Task<CardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            var cardMember = await _unitOfWork.CardMemberRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_MEMBER_FIELD, ErrorMessage.CARD_MEMBER_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            cardMember.UpdatedDate = DateTime.UtcNow;
            cardMember.UpdatedUser = currentUserId;
            cardMember.IsActive = isActive;

            _unitOfWork.CardMemberRepository.Update(cardMember);

            //
            var existingUser = await _userService.GetUserIdByCardMemberIdAsync(Id);
            if (existingUser == Guid.Empty)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            //
            await _notificationService.SendNotificationAsync(existingUser, "You have been removed to a card!", $"You have removed to the card.");

            await _unitOfWork.SaveChangesAsync();
           
            var mappedCardMember = _mapper.Map<CardMemberDetail>(cardMember);
            return mappedCardMember;
        }

        public async Task<CardMember> GetCardMemberByUserIdAsync(Guid cardId, Guid userId)
        {
            return await _unitOfWork.CardMemberRepository.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.CardId == cardId);
        }
    }
}
