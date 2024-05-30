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
using Trello.Application.DTOs.CardMember;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
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

        public CardMemberService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CardMemberDetail> CreateCardMemberAsync(CardMemberDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingCard = await GetCardById(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }
            var existingUser = await GetUserById(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var cardMember = _mapper.Map<CardMember>(requestBody);
            cardMember.Id = Guid.NewGuid();
            cardMember.CreatedDate = DateTime.Now;
            cardMember.CreatedUser = currentUserId;
            cardMember.IsActive = true;

            await _unitOfWork.CardMemberRepository.InsertAsync(cardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardMemberDto = _mapper.Map<CardMemberDetail>(cardMember);

            return createdBoardMemberDto;
        }

        public async Task<List<CardMemberDetail>> GetAllCardMemberAsync(Guid cardId, string? userName)
        {
            IQueryable<CardMember> cardsQuery = _unitOfWork.CardMemberRepository.GetAll();

            cardsQuery = cardsQuery.Where(u => u.CardId == cardId);

            if (!string.IsNullOrEmpty(userName))
            {
                var user = await GetUserByUserName(userName);
                if (user == null)
                {
                    return new List<CardMemberDetail>();
                }
                cardsQuery = cardsQuery.Where(u => u.UserId == user.Id);
            }

            List<CardMemberDetail> lists = await cardsQuery
                .Select(u => _mapper.Map<CardMemberDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<Card> GetCardById(Guid cardId)
        {
            return await _unitOfWork.CardRepository.GetByIdAsync(cardId);
        }
        public async Task<User> GetUserById(Guid userId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }
        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _unitOfWork.UserRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
