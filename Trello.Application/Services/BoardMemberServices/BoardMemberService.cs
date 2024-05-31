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
using System.Xml.Linq;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
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

            var existingBoard = await GetBoardById(requestBody.BoardId);
            if (existingBoard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            var existingUser = await GetUserById(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var boardMember = _mapper.Map<BoardMember>(requestBody);
            boardMember.Id = Guid.NewGuid();
            boardMember.CreatedDate = DateTime.Now;
            boardMember.CreatedUser = currentUserId;
            boardMember.IsActive = true;

            await _unitOfWork.BoardMemberRepository.InsertAsync(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardMemberDto = _mapper.Map<BoardMemberDetail>(boardMember);

            return createdBoardMemberDto;
        }
        public async Task<List<BoardMemberDetail>> GetAllBoardMemberAsync(Guid boardId, string? name)
        {
            IQueryable<BoardMember> boardMembersQuery = _unitOfWork.BoardMemberRepository.GetAll();

            boardMembersQuery = boardMembersQuery.Where(u => u.BoardId == boardId);

            if (!string.IsNullOrEmpty(name))
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.User.Name.Contains(name));
            }

            List<BoardMemberDetail> lists = await boardMembersQuery
                .Select(bm => _mapper.Map<BoardMemberDetail>(bm))
                .ToListAsync();

            return lists;
        }
        public async Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, BoardMemberDTO requestBody)
        {

            var boardMember = await _unitOfWork.BoardMemberRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            boardMember = _mapper.Map(requestBody, boardMember);
            boardMember.UpdatedDate = DateTime.Now;
            boardMember.UpdatedUser = currentUserId;

            _unitOfWork.BoardMemberRepository.Update(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var boardMemberDetail = _mapper.Map<BoardMemberDetail>(boardMember);
            return boardMemberDetail;
        }
        public async Task<BoardMemberDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            var boardMember = await _unitOfWork.BoardMemberRepository.GetByIdAsync(Id);
            if (boardMember == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            boardMember.UpdatedDate = DateTime.Now;
            boardMember.UpdatedUser = currentUserId;
            boardMember.IsActive = isActive;

            _unitOfWork.BoardMemberRepository.Update(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoard = _mapper.Map<BoardMemberDetail>(boardMember);
            return mappedBoard;
        }
        public async Task<Board> GetBoardById(Guid boardId)
        {
            return await _unitOfWork.BoardRepository.GetByIdAsync(boardId);
        }
        public async Task<User> GetUserById(Guid userId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }
    }
}
