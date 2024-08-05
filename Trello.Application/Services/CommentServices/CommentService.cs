using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;
using Trello.Application.DTOs.Comment;
using Trello.Application.Services.CardServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Trello.Application.Utilities.Helper.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using Trello.Domain.Enums;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.DTOs.BoardMember;

namespace Trello.Application.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        private readonly IBoardService _boardService;
        private readonly IBoardMemberService _boardMemberService;
        private readonly IHubContext<SignalHub> _hubContext;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICardService cardService,
            IHubContext<SignalHub> hubContext, IBoardService boardService, IBoardMemberService boardMemberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _hubContext = hubContext;
            _boardService = boardService;
            _boardMemberService = boardMemberService;
        }

        public async Task<CommentDetail> CreateCommentAsync(CommentDTO requestBody)
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

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the request body to a Comment entity and set metadata
            var comment = _mapper.Map<Comment>(requestBody);
            comment.Id = Guid.NewGuid();
            comment.UserId = currentUserId;
            comment.IsActive = true;
            comment.CreatedDate = DateTime.UtcNow;
            comment.CreatedUser = currentUserId;

            // Insert the new comment into the repository
            await _unitOfWork.CommentRepository.InsertAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            // Map the created comment to a CommentDetail DTO
            var createdCommentDto = _mapper.Map<CommentDetail>(comment);

            // Get the board associated with the card
            var board = await _boardService.GetBoardByCardIdAsync(existingCard.Id);
            if (board == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            // Get all members of the board
            var boardMembers = await _boardMemberService.GetAllBoardMemberAsync(board.Id);

            // Notify all board members about the new comment
            foreach (var member in boardMembers)
            {
                await _hubContext.Clients.All.SendAsync(SignalRHubEnum.ReceiveComment.ToString(), createdCommentDto);
            }

            return createdCommentDto;
        }

        public async Task<List<CommentDetail>> GetAllCommentAsync(Guid cardId)
        {
            // Query to get all active comments for a specific card
            IQueryable<Comment> commentsQuery = _unitOfWork.CommentRepository.GetAll();
            commentsQuery = commentsQuery.Where(u => u.CardId == cardId && u.IsActive);

            // Map the comments to CommentDetail DTOs
            List<CommentDetail> lists = await commentsQuery
                .Select(cm => new CommentDetail
                {
                    Id = cm.Id,
                    CardId = cm.CardId,
                    UserId = cm.UserId,
                    UserName = cm.User.Name,
                    Content = cm.Content,
                    CreatedDate = cm.CreatedDate,
                    CreatedUser = cm.CreatedUser,
                    UpdatedDate = cm.UpdatedDate,
                    UpdatedUser = cm.UpdatedUser,
                    IsActive = cm.IsActive
                })
                .ToListAsync();

            return lists;
        }

        public async Task<CommentDetail> UpdateCommentAsync(Guid id, string content)
        {
            // Get the comment by ID and throw an exception if it doesn't exist
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.COMMENT_FIELD, ErrorMessage.COMMENT_NOT_EXIST);

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the comment's content and metadata
            comment.UpdatedDate = DateTime.UtcNow;
            comment.UpdatedUser = currentUserId;
            comment.Content = content;

            // Update the comment in the repository
            _unitOfWork.CommentRepository.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated comment to a CommentDetail DTO
            var commentDetail = _mapper.Map<CommentDetail>(comment);

            // Get the board associated with the card
            var existingCard = await _cardService.GetCardByIdAsync(comment.CardId);
            var board = await _boardService.GetBoardByCardIdAsync(existingCard.Id);
            if (board == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            // Get all members of the board
            var boardMembers = await _boardMemberService.GetAllBoardMemberAsync(board.Id);

            // Notify all board members about the updated comment
            foreach (var member in boardMembers)
            {
                await _hubContext.Clients.User(member.UserId.ToString()).SendAsync(SignalRHubEnum.UpdateComment.ToString(), commentDetail);
            }

            return commentDetail;
        }

        public async Task<CommentDetail> ChangeStatusAsync(Guid id, bool isActive)
        {
            // Get the comment by ID and throw an exception if it doesn't exist
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.COMMENT_FIELD, ErrorMessage.COMMENT_NOT_EXIST);

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the comment's status and metadata
            comment.UpdatedDate = DateTime.UtcNow;
            comment.UpdatedUser = currentUserId;
            comment.IsActive = isActive;

            // Update the comment in the repository
            _unitOfWork.CommentRepository.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated comment to a CommentDetail DTO
            var mappedComment = _mapper.Map<CommentDetail>(comment);

            // Get the board associated with the card
            var existingCard = await _cardService.GetCardByIdAsync(comment.CardId);
            var board = await _boardService.GetBoardByCardIdAsync(existingCard.Id);
            if (board == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            // Get all members of the board
            var boardMembers = await _boardMemberService.GetAllBoardMemberAsync(board.Id);

            // Notify all board members about the updated comment status
            foreach (var member in boardMembers)
            {
                await _hubContext.Clients.User(member.UserId.ToString()).SendAsync(SignalRHubEnum.UpdateComment.ToString(), mappedComment);
            }

            return mappedComment;
        }

    }
}
