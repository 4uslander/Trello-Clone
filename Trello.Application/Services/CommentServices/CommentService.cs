using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Comment;
using Trello.Application.DTOs.List;
using Trello.Application.Services.CardServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICardService cardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
        }

        public async Task<CommentDetail> CreateCommentAsync(CommentDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var comment = _mapper.Map<Comment>(requestBody);
            comment.Id = Guid.NewGuid();
            comment.UserId = currentUserId;
            comment.IsActive = true;
            comment.CreatedDate = DateTime.Now;
            comment.CreatedUser = currentUserId;

            await _unitOfWork.CommentRepository.InsertAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            var createdCommentDto = _mapper.Map<CommentDetail>(comment);
            return createdCommentDto;
        }
    }
}
