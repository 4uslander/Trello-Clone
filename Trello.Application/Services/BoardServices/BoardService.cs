using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.Services.CardServices;
using Trello.Application.Services.ListServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.BoardServices
{
    public class BoardService : IBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        private readonly IListService _listService;
        private readonly IRoleService _roleService;

        public BoardService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICardService cardService, IListService listService, IRoleService roleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _listService = listService;
            _roleService = roleService;
        }

        public async Task<BoardDetail> CreateBoardAsync(BoardDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var board = _mapper.Map<Board>(requestBody);
            board.Id = Guid.NewGuid();
            board.CreatedDate = DateTime.Now;
            board.CreatedUser = currentUserId;
            board.IsPublic = true;
            board.IsActive = true;
            await _unitOfWork.BoardRepository.InsertAsync(board);
            await _unitOfWork.SaveChangesAsync();

            var adminRole = await _roleService.GetRoleByNameAsync(BoardMemberRoleEnum.Admin.ToString());
            if (adminRole == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);
            }

            var boardMember = new BoardMember
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                UserId = currentUserId,
                RoleId = adminRole.Id,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedUser = currentUserId
            };

            await _unitOfWork.BoardMemberRepository.InsertAsync(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardDto = _mapper.Map<BoardDetail>(board);
            return createdBoardDto;
        }

        public async Task<List<BoardDetail>> GetBoardAsync()
        {
            // Get the current user
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Query to get all boards
            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll();

            // Filter for active boards and either public boards or private boards created by the current user
            boardsQuery = boardsQuery.Where(u => u.IsActive && (u.IsPublic || u.CreatedUser == currentUserId));

            // Map the boards to the BoardDetail DTO
            List<BoardDetail> boards = await boardsQuery
                .Select(u => _mapper.Map<BoardDetail>(u))
                .ToListAsync();
            return boards;
        }

        public async Task<List<BoardDetail>> GetBoardByFilterAsync(string? name, Guid? createdUser, Guid? updatedUser,
            DateTime? createdDate, DateTime? updatedDate, bool? isPublic, bool? isActive)
        {
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll();

            if (!string.IsNullOrEmpty(name))
            {
                boardsQuery = boardsQuery.Where(u => u.Name.Contains(name));
            }

            if (createdUser.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.CreatedUser == createdUser.Value);
            }

            if (updatedUser.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.UpdatedUser == updatedUser.Value);
            }

            if (createdDate.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.CreatedDate.Date == createdDate.Value.Date);
            }

            if (updatedDate.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.UpdatedDate.Value.Date == updatedDate.Value.Date);
            }

            if (isPublic.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.IsPublic == isPublic.Value);
            }

            if (isActive.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.IsActive == isActive.Value);
            }

            List<BoardDetail> boards = await boardsQuery
                .Select(b => _mapper.Map<BoardDetail>(b))
                .ToListAsync();
            return boards;
        }

        public async Task<BoardDetail> UpdateBoardAsync(Guid id, BoardDTO requestBody)
        {
            var board = await GetBoardByIdAsync(id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            var existingBoard = await GetBoardByNameAsync(requestBody.Name);
            if (existingBoard != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_ALREADY_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            board = _mapper.Map(requestBody, board);
            board.UpdatedDate = DateTime.Now;
            board.UpdatedUser = currentUserId;
            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var boardDetail = _mapper.Map<BoardDetail>(board);
            return boardDetail;
        }

        public async Task<BoardDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            var board = await GetBoardByIdAsync(Id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            board.UpdatedDate = DateTime.Now;
            board.UpdatedUser = currentUserId;
            board.IsActive = isActive;

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoard = _mapper.Map<BoardDetail>(board);
            return mappedBoard;
        }

        public async Task<BoardDetail> ChangeVisibilityAsync(Guid id, bool isPublic)
        {
            var board = await GetBoardByIdAsync(id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            board.UpdatedDate = DateTime.Now;
            board.UpdatedUser = currentUserId;
            board.IsPublic = isPublic;

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoard = _mapper.Map<BoardDetail>(board);
            return mappedBoard;
        }


        public async Task<Board> GetBoardByNameAsync(string name)
        {
            return await _unitOfWork.BoardRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<Board> GetBoardByIdAsync(Guid id)
        {
            return await _unitOfWork.BoardRepository.GetByIdAsync(id);
        }

        public async Task<Board> GetBoardByCardIdAsync(Guid cardId)
        {
            var card = await _cardService.GetCardByIdAsync(cardId);
            if (card == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            var list = await _listService.GetListByIdAsync(card.ListId);
            if (list == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
            }

            return await GetBoardByIdAsync(list.BoardId);
        }

    }
}
