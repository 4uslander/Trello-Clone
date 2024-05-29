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

            await IsExistBoardName(requestBody.Name);

            var currentUserIdGuid = GetUserAuthorizationId.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var board = _mapper.Map<Board>(requestBody);
            board.Id = Guid.NewGuid();
            board.CreatedDate = DateTime.Now;
            board.CreatedUser = currentUserIdGuid;
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
            var currentUserIdGuid = GetUserAuthorizationId.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Query to get all boards
            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll();

            // Filter for active boards and either public boards or private boards created by the current user
            boardsQuery = boardsQuery.Where(u => u.IsActive && (u.IsPublic || u.CreatedUser == currentUserIdGuid));

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
            var board = await IsExistBoardId(id);

            await IsExistBoardName(requestBody.Name);

            var currentUserIdGuid = GetUserAuthorizationId.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserIdGuid)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            board = _mapper.Map(requestBody, board);
            board.UpdatedDate = DateTime.Now;
            board.UpdatedUser = currentUserIdGuid;
            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var boardDetail = _mapper.Map<BoardDetail>(board);
            return boardDetail;
        }

        public async Task<BoardDetail> ChangeStatusAsync(Guid Id)
        {
            var board = await IsExistBoardId(Id);

            var currentUserIdGuid = GetUserAuthorizationId.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserIdGuid)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            board.UpdatedDate = DateTime.Now;
            board.UpdatedUser = currentUserIdGuid;
            if (board.IsActive == true)
            {
                board.IsActive = false;
            }
            else
            {
                board.IsActive = true;
            }

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoard = _mapper.Map<BoardDetail>(board);
            return mappedBoard;
        }

        public async Task<BoardDetail> ChangeVisibility(Guid Id)
        {
            var board = await IsExistBoardId(Id);

            var currentUserIdGuid = GetUserAuthorizationId.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            if (board.CreatedUser != currentUserIdGuid)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this board");
            }

            board.UpdatedDate = DateTime.Now;
            board.UpdatedUser = currentUserIdGuid;
            if (board.IsPublic == true)
            {
                board.IsPublic = false;
            }
            else
            {
                board.IsPublic = true;
            }

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoard = _mapper.Map<BoardDetail>(board);
            return mappedBoard;
        }

        public async System.Threading.Tasks.Task IsExistBoardName(string? name)
        {
            var isExist = await _unitOfWork.BoardRepository.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_ALREADY_EXIST);
        }

        public async Task<Board> IsExistBoardId(Guid? id)
        {
            var board = await _unitOfWork.BoardRepository.GetByIdAsync(id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            return board;
        }
    }
}
