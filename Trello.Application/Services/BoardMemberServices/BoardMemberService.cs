using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.BoardMemberServices
{
    public class BoardMemberService : IBoardMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BoardMemberService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BoardMemberDetail> CreateBoardMemberAsync(CreateBoardMemberDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            await IsExistBoard(requestBody.BoardId);
            await IsExistUser(requestBody.UserId);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(requestBody.CreatedUserId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var boardMember = _mapper.Map<BoardMember>(requestBody);
            boardMember.Id = Guid.NewGuid();
            boardMember.CreatedDate = DateTime.Now;
            boardMember.CreatedUser = user.Id;
            boardMember.IsActive = true;

            await _unitOfWork.BoardMemberRepository.InsertAsync(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardMemberDto = _mapper.Map<BoardMemberDetail>(boardMember);

            return createdBoardMemberDto;
        }
        public List<BoardMemberDetail> GetAllBoardMember(string? name)
        {
            IQueryable<BoardMember> boardMembersQuery = _unitOfWork.BoardMemberRepository.GetAll();

            if (!string.IsNullOrEmpty(name))
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.User.Name.Contains(name));
            }

            List<BoardMemberDetail> lists = boardMembersQuery
                .Select(bm => _mapper.Map<BoardMemberDetail>(bm))
                .ToList();

            return lists;
        }
        public async Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, BoardMemberDTO requestBody)
        {

            var boardMember = await _unitOfWork.BoardMemberRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);


            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            boardMember = _mapper.Map(requestBody, boardMember);
            boardMember.UpdatedDate = DateTime.Now;
            boardMember.UpdatedUser = Guid.Parse(currentUserId);

            _unitOfWork.BoardMemberRepository.Update(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var boardMemberDetail = _mapper.Map<BoardMemberDetail>(boardMember);
            return boardMemberDetail;
        }
        public async Task<BoardMemberDetail> ChangeStatusAsync(Guid Id)
        {
            var boardMember = await _unitOfWork.BoardMemberRepository.GetByIdAsync(Id);
            if (boardMember == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            boardMember.UpdatedDate = DateTime.Now;
            boardMember.UpdatedUser = Guid.Parse(currentUserId);

            if (boardMember.IsActive == true)
            {
                boardMember.IsActive = false;
            }
            else
            {
                boardMember.IsActive = true;
            }

            _unitOfWork.BoardMemberRepository.Update(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoard = _mapper.Map<BoardMemberDetail>(boardMember);
            return mappedBoard;
        }
        public async System.Threading.Tasks.Task IsExistBoard(Guid boardId)
        {
            var isExist = await _unitOfWork.BoardRepository.AnyAsync(x => x.Id.Equals(boardId));
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
