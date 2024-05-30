using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Trello.Application.DTOs.Board;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
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

        public BoardService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BoardDetail> CreateBoardAsync(BoardDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingBoard = await GetBoardByName(requestBody.Name);
            if (existingBoard != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_ALREADY_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var board = _mapper.Map<Board>(requestBody);
            board.Id = Guid.NewGuid();
            board.CreatedDate = DateTime.Now;
            board.CreatedUser = currentUserId;
            board.IsPublic = true;
            board.IsActive = true;
            await _unitOfWork.BoardRepository.InsertAsync(board);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardDto = _mapper.Map<BoardDetail>(board);
            return createdBoardDto;
        }

        public async Task<List<BoardDetail>> GetAllBoardAsync(string? name)
        {
            // Get the current user
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Query to get all boards
            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll();

            // Filter for active boards and either public boards or private boards created by the current user
            boardsQuery = boardsQuery.Where(u => u.IsActive && (u.IsPublic || u.CreatedUser == currentUserId));

            // Additional filter by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                boardsQuery = boardsQuery.Where(u => u.Name.Contains(name));
            }

            // Map the boards to the BoardDetail DTO
            List<BoardDetail> boards = await boardsQuery
                .Select(u => _mapper.Map<BoardDetail>(u))
                .ToListAsync();
            return boards;
        }

        public async Task<BoardDetail> UpdateBoardAsync(Guid id, BoardDTO requestBody)
        {
            var board = await GetBoardById(id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            var existingBoard = await GetBoardByName(requestBody.Name);
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
            var board = await GetBoardById(Id);
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
            var board = await GetBoardById(id);
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


        public async Task<Board> GetBoardByName(string? name)
        {
            return await _unitOfWork.BoardRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<Board> GetBoardById(Guid? id)
        {
            return await _unitOfWork.BoardRepository.GetByIdAsync(id);
        }
    }
}
