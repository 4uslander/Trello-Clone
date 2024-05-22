using AutoMapper;
using System.Net;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.User;
using Trello.Application.Utilities.ErrorHandler;
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

        public BoardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<BoardDetail> CreateBoardAsync(CreateBoardDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            await IsExistBoardName(requestBody.Name);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(requestBody.CreatedUserId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var board = _mapper.Map<Board>(requestBody);

            board.CreatedDate = DateTime.Now;
            board.CreatedUser = user.Name;
            board.IsPublic = true;
            board.IsActive = true;

            await _unitOfWork.BoardRepository.InsertAsync(board);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardDto = _mapper.Map<BoardDetail>(board);

            return createdBoardDto;
        }

        public List<BoardDetail> GetAllBoard(SearchBoardDTO searchKey)
        {
            IQueryable<Board> boardsQuery = _unitOfWork.BoardRepository.GetAll();


            if (!string.IsNullOrEmpty(searchKey?.Name))
            {
                boardsQuery = boardsQuery.Where(u => u.Name.Contains(searchKey.Name));
            }

            List<BoardDetail> boards = boardsQuery
                .Select(u => _mapper.Map<BoardDetail>(u))
                .ToList();

            return boards;
        }

        public async System.Threading.Tasks.Task IsExistBoardName(string? name)
        {
            var isExist = await _unitOfWork.BoardRepository.AnyAsync(x => x.Name.Equals(name));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_ALREADY_EXIST);
        }

        public async Task<BoardDetail> UpdateBoardAsync(int id, UpdateBoardDTO requestBody)
        {
            if (id != requestBody.BoardId)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

            var board = await _unitOfWork.BoardRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);


            board = _mapper.Map(requestBody, board);

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            var boardDetail = _mapper.Map<BoardDetail>(board);
            return boardDetail;
        }

        public async Task<BoardDetail> ChangeStatusAsync(int Id)
        {
            var board = await _unitOfWork.BoardRepository.GetByIdAsync(Id);
            if (board == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_ID_FIELD, ErrorMessage.BOARD_NOT_EXIST);

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

    }
}
