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
            // Validate the request body
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the request body to a Board entity
            var board = _mapper.Map<Board>(requestBody);
            board.Id = Guid.NewGuid();
            board.CreatedDate = DateTime.UtcNow;
            board.CreatedUser = currentUserId;
            board.IsPublic = false;
            board.IsActive = true;

            // Insert the new board into the repository
            await _unitOfWork.BoardRepository.InsertAsync(board);
            await _unitOfWork.SaveChangesAsync();

            // Get the admin role for the board member
            var adminRole = await _roleService.GetRoleByNameAsync(BoardMemberRoleEnum.Admin.ToString());
            if (adminRole == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);
            }

            // Create a new board member with the admin role
            var boardMember = new BoardMember
            {
                Id = Guid.NewGuid(),
                BoardId = board.Id,
                UserId = currentUserId,
                RoleId = adminRole.Id,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedUser = currentUserId
            };

            // Insert the new board member into the repository
            await _unitOfWork.BoardMemberRepository.InsertAsync(boardMember);
            await _unitOfWork.SaveChangesAsync();

            // Map the created board entity to a BoardDetail DTO
            var createdBoardDto = _mapper.Map<BoardDetail>(board);
            return createdBoardDto;
        }

        public async Task<List<BoardDetail>> GetBoardAsync()
        {
            // Get the current user ID
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

        public async Task<List<BoardDetail>> GetBoardByFilterAsync(string? name, bool? isPublic, bool? isActive)
        {
            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Query to get all boards
            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll();

            // Filter for boards created by the current user
            boardsQuery = boardsQuery.Where(u => u.CreatedUser == currentUserId);

            // Apply additional filters based on the provided parameters
            if (!string.IsNullOrEmpty(name))
            {
                boardsQuery = boardsQuery.Where(u => u.Name.Contains(name));
            }

            if (isPublic.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.IsPublic == isPublic.Value);
            }

            if (isActive.HasValue)
            {
                boardsQuery = boardsQuery.Where(u => u.IsActive == isActive.Value);
            }

            // Map the filtered boards to the BoardDetail DTO
            List<BoardDetail> boards = await boardsQuery
                .Select(b => _mapper.Map<BoardDetail>(b))
                .ToListAsync();
            return boards;
        }

        public async Task<List<BoardDetail>> GetBoardsByCurrentUserMembershipAsync()
        {
            // Get the current user ID
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Query to get boards where the current user is a member
            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll()
                .Join(_unitOfWork.BoardMemberRepository.GetAll(),
                      board => board.Id,
                      boardMember => boardMember.BoardId,
                      (board, boardMember) => new { board, boardMember })
                .Where(bb => bb.boardMember.UserId == currentUserId && bb.boardMember.IsActive)
                .Select(bb => bb.board);

            // Filter for active boards
            boardsQuery = boardsQuery.Where(u => u.IsActive);

            // Map the boards to the BoardDetail DTO
            List<BoardDetail> boards = await boardsQuery
                .Select(u => _mapper.Map<BoardDetail>(u))
                .ToListAsync();

            return boards;
        }

        public async Task<BoardDetail> UpdateBoardAsync(Guid id, BoardDTO requestBody)
        {
            // Get the board by ID
            var board = await GetBoardByIdAsync(id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            // Check if a board with the same name already exists
            var existingBoard = await GetBoardByNameAsync(requestBody.Name);
            if (existingBoard != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_ALREADY_EXIST);
            }

            // Get the current user ID and check if they have permission to update the board
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            // Map the request body to the existing board entity
            board = _mapper.Map(requestBody, board);
            board.UpdatedDate = DateTime.UtcNow;
            board.UpdatedUser = currentUserId;

            // Update the board in the repository
            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated board entity to a BoardDetail DTO
            var boardDetail = _mapper.Map<BoardDetail>(board);
            return boardDetail;
        }

        public async Task<BoardDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            // Get the board by ID
            var board = await GetBoardByIdAsync(Id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            // Get the current user ID and check if they have permission to update the board
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            // Update the status and metadata of the board
            board.UpdatedDate = DateTime.UtcNow;
            board.UpdatedUser = currentUserId;
            board.IsActive = isActive;

            // Update the board in the repository
            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated board entity to a BoardDetail DTO
            var mappedBoard = _mapper.Map<BoardDetail>(board);
            return mappedBoard;
        }

        public async Task<BoardDetail> ChangeVisibilityAsync(Guid id, bool isPublic)
        {
            // Get the board by ID
            var board = await GetBoardByIdAsync(id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            // Get the current user ID and check if they have permission to update the board
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            // Update the visibility and metadata of the board
            board.UpdatedDate = DateTime.UtcNow;
            board.UpdatedUser = currentUserId;
            board.IsPublic = isPublic;

            // Update the board in the repository
            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated board entity to a BoardDetail DTO
            var mappedBoard = _mapper.Map<BoardDetail>(board);
            return mappedBoard;
        }

        public async Task<Board> GetBoardByNameAsync(string name)
        {
            // Get a board by its name
            return await _unitOfWork.BoardRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<Board> GetBoardByIdAsync(Guid id)
        {
            // Get a board by its ID
            return await _unitOfWork.BoardRepository.GetByIdAsync(id);
        }

        public async Task<Board> GetBoardByCardIdAsync(Guid cardId)
        {
            // Get the card by its ID
            var card = await _cardService.GetCardByIdAsync(cardId);
            if (card == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            // Get the list associated with the card
            var list = await _listService.GetListByIdAsync(card.ListId);
            if (list == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
            }

            // Get the board associated with the list
            return await GetBoardByIdAsync(list.BoardId);
        }
    }
}
