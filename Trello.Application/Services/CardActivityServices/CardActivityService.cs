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
        private readonly ICardService _cardService;
        private readonly IUserService _userService;
        private readonly ICardMemberService _cardMemberService;
        private readonly IBoardService _boardService;
        private readonly IBoardMemberService _boardMemberService;
        private readonly IHubContext<SignalHub> _hubContext;

        public CardActivityService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICardService cardService, IUserService userService, ICardMemberService cardMemberService, 
            IBoardService boardService, IBoardMemberService boardMemberService, IHubContext<SignalHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _userService = userService; //
            _cardMemberService = cardMemberService; //
            _boardService = boardService;
            _boardMemberService = boardMemberService;
            _hubContext = hubContext;
        }

        public async Task<CardActivityDetail> CreateCardActivityAsync(CreateCardActivityDTO requestBody)
        {
            if(requestBody == null)
            {
                 throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }


            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var activity = _mapper.Map<CardActivity>(requestBody);
            activity.Id = Guid.NewGuid();
            activity.UserId = currentUserId;
            activity.CreatedDate = DateTime.UtcNow;
            activity.CreatedUser = currentUserId;
            activity.IsActive = true;

            await _unitOfWork.CardActivityRepository.InsertAsync(activity);
            await _unitOfWork.SaveChangesAsync();

            var createdCardActivityDto = _mapper.Map<CardActivityDetail>(activity);

            var board = await _boardService.GetBoardByCardIdAsync(existingCard.Id);
            if(board == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            var boardMembers = await _boardMemberService.GetAllBoardMemberAsync(board.Id);
            foreach (var member in boardMembers)
            {
                await _hubContext.Clients.User(member.UserId.ToString()).SendAsync(SignalRHubEnum.ReceiveActivity.ToString(), createdCardActivityDto);
            }

            return createdCardActivityDto;


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
