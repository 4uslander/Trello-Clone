using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.CardMember;
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

            await IsExistCard(requestBody.CardId);
            await IsExistUser(requestBody.UserId);

            var currentUserIdGuid = GetUserAuthorizationId.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var cardMember = _mapper.Map<CardMember>(requestBody);
            cardMember.Id = Guid.NewGuid();
            cardMember.CreatedDate = DateTime.Now;
            cardMember.CreatedUser = currentUserIdGuid;
            cardMember.IsActive = true;

            await _unitOfWork.CardMemberRepository.InsertAsync(cardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardMemberDto = _mapper.Map<CardMemberDetail>(cardMember);

            return createdBoardMemberDto;
        }

        public async System.Threading.Tasks.Task IsExistCard(Guid cardId)
        {
            var isExist = await _unitOfWork.BoardRepository.AnyAsync(x => x.Id.Equals(cardId));
            if (!isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
        }
        public async System.Threading.Tasks.Task IsExistUser(Guid userId)
        {
            var isExist = await _unitOfWork.UserRepository.AnyAsync(x => x.Id.Equals(userId));
            if (!isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
        }
    }
}
