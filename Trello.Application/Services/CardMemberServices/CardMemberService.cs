using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Trello.Application.DTOs.CardActivity;
using Trello.Application.DTOs.CardMember;
using Trello.Application.DTOs.Notification;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.CardActivityServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.NotificationServices;
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
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly INotificationService _notificationService;
        private readonly ICardActivityService _cardActivityService;

        public CardMemberService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICardService cardService, IUserService userService, IBoardMemberService boardMemberService,
            IFirebaseNotificationService firebaseNotificationService, INotificationService notificationService, ICardActivityService cardActivityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _userService = userService;
            _boardMemberService = boardMemberService;
            _firebaseNotificationService = firebaseNotificationService;
            _notificationService = notificationService;
            _cardActivityService = cardActivityService;
        }

        public async Task<CardMemberDetail> CreateCardMemberAsync(CardMemberDTO requestBody)
        {
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Verify that the card exists
            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            // Verify that the user exists
            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            // Verify that the user is a member of the board
            var existingBoardMember = await _boardMemberService.GetBoardMemberByUserIdAsync(requestBody.UserId);
            if (existingBoardMember == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);
            }

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the request body to a CardMember entity and set metadata
            var cardMember = _mapper.Map<CardMember>(requestBody);
            cardMember.Id = Guid.NewGuid();
            cardMember.CreatedDate = DateTime.UtcNow;
            cardMember.CreatedUser = currentUserId;
            cardMember.IsActive = true;

            // Insert the new card member into the repository
            await _unitOfWork.CardMemberRepository.InsertAsync(cardMember);

            // Map the created card member to a CardMemberDetail DTO
            var createdBoardMemberDto = _mapper.Map<CardMemberDetail>(cardMember);

            // Create a notification for the new card member
            var notificationRequest = new NotificationDTO
            {
                UserId = requestBody.UserId,
                Title = NotificationTitleField.ADDED_TO_CARD,
                Body = $"\n{NotificationBodyField.ADDED_TO_CARD}: {existingCard.Title}."
            };

            // Create the notification and send it via Firebase
            var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);
            await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);

            // 
            var cardActivityRequest = new CreateCardActivityDTO
            {
                Activity = "Joined this card",
                CardId = requestBody.CardId,
                UserId = requestBody.UserId
            };
            await _cardActivityService.CreateCardActivityAsync(cardActivityRequest);
            
           

            await _unitOfWork.SaveChangesAsync();
            return createdBoardMemberDto;
        }

        public async Task<List<CardMemberDetail>> GetAllCardMemberAsync(Guid cardId)
        {
            // Query to get all active card members for a specific card
            IQueryable<CardMember> cardsQuery = _unitOfWork.CardMemberRepository.GetAll();
            cardsQuery = cardsQuery.Where(u => u.CardId == cardId && u.IsActive);

            // Map the card members to CardMemberDetail DTOs
            List<CardMemberDetail> lists = await cardsQuery
                .Select(u => _mapper.Map<CardMemberDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<List<CardMemberDetail>> GetCardMemberByFilterAsync(Guid cardId, string? userName, bool? isActive)
        {
            // Query to get all card members for a specific card
            IQueryable<CardMember> cardsQuery = _unitOfWork.CardMemberRepository.GetAll();
            cardsQuery = cardsQuery.Where(cm => cm.CardId == cardId);

            // Apply filters based on the provided parameters
            if (!string.IsNullOrEmpty(userName))
            {
                cardsQuery = cardsQuery.Where(cm => cm.User.Name.Contains(userName));
            }

            if (isActive.HasValue)
            {
                cardsQuery = cardsQuery.Where(cm => cm.IsActive == isActive.Value);
            }

            // Map the filtered card members to CardMemberDetail DTOs
            List<CardMemberDetail> lists = await cardsQuery
                .Select(cm => _mapper.Map<CardMemberDetail>(cm))
                .ToListAsync();

            return lists;
        }

        public async Task<CardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            // Get the card member by ID and throw an exception if it doesn't exist
            var cardMember = await _unitOfWork.CardMemberRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_MEMBER_FIELD, ErrorMessage.CARD_MEMBER_NOT_EXIST);

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the card member's status and metadata
            cardMember.UpdatedDate = DateTime.UtcNow;
            cardMember.UpdatedUser = currentUserId;
            cardMember.IsActive = isActive;

            // Update the card member in the repository
            _unitOfWork.CardMemberRepository.Update(cardMember);

            // Get the user associated with the card member and throw an exception if the user doesn't exist
            var existingUser = await _userService.GetUserIdByCardMemberIdAsync(Id);
            if (existingUser == Guid.Empty)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            // Create a notification for the status change
            var notificationRequest = new NotificationDTO
            {
                UserId = existingUser,
                Title = NotificationTitleField.CARD_MEMBER_REMOVED,
                Body = $"\n{NotificationBodyField.CARD_MEMBER_REMOVED}"
            };

            // Create the notification and send it via Firebase
            var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);
            await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);

            // Save the changes to the repository
            await _unitOfWork.SaveChangesAsync();

            // Map the updated card member to a CardMemberDetail DTO
            var mappedCardMember = _mapper.Map<CardMemberDetail>(cardMember);
            return mappedCardMember;
        }

        public async Task<CardMember> GetCardMemberByUserIdAsync(Guid cardId, Guid userId)
        {
            // Get the card member by card ID and user ID
            return await _unitOfWork.CardMemberRepository.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.CardId == cardId);
        }

    }
}
