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
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Enums;
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
        private readonly IBoardService _boardService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public BoardMemberService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IBoardService boardService, IUserService userService, IRoleService roleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _boardService = boardService;
            _userService = userService;
            _roleService = roleService;
        }
        public async Task<BoardMemberDetail> CreateBoardMemberAsync(BoardMemberDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingBoard = await _boardService.GetBoardByIdAsync(requestBody.BoardId);
            if (existingBoard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            var existingUser = await _userService.GetUserByIdAsync(requestBody.UserId);
            if (existingUser == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var memberRole = await _roleService.GetRoleByNameAsync(BoardMemberRoleEnum.Member.ToString());
            if (memberRole == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var boardMember = _mapper.Map<BoardMember>(requestBody);
            boardMember.Id = Guid.NewGuid();
            boardMember.CreatedDate = DateTime.Now;
            boardMember.CreatedUser = currentUserId;
            boardMember.RoleId = memberRole.Id;
            boardMember.IsActive = true;

            await _unitOfWork.BoardMemberRepository.InsertAsync(boardMember);
            await _unitOfWork.SaveChangesAsync();

            var createdBoardMemberDto = _mapper.Map<BoardMemberDetail>(boardMember);

            return createdBoardMemberDto;
        }
        public async Task<List<BoardMemberDetail>> GetAllBoardMemberAsync(Guid boardId)
        {
            IQueryable<BoardMember> boardMembersQuery = _unitOfWork.BoardMemberRepository.GetAll();

            boardMembersQuery = boardMembersQuery.Where(u => u.BoardId == boardId);

            List<BoardMemberDetail> lists = await boardMembersQuery
                .Select(bm => _mapper.Map<BoardMemberDetail>(bm))
                .ToListAsync();

            return lists;
        }

        public async Task<List<BoardMemberDetail>> GetBoardMemberByFilterAsync(Guid boardId, string? name, Guid? userId, Guid? roleId,
            Guid? createdUser, Guid? updatedUser, DateTime? createdDate, DateTime? updatedDate, bool? isActive)
        {
            IQueryable<BoardMember> boardMembersQuery = _unitOfWork.BoardMemberRepository.GetAll();

            boardMembersQuery = boardMembersQuery.Where(bm => bm.BoardId == boardId);

            if (!string.IsNullOrEmpty(name))
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.User.Name.Contains(name));
            }

            if (userId.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.UserId == userId.Value);
            }

            if (roleId.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.RoleId == roleId.Value);
            }

            if (createdUser.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.CreatedUser == createdUser.Value);
            }

            if (updatedUser.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.UpdatedUser == updatedUser.Value);
            }

            if (createdDate.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.CreatedDate.Date == createdDate.Value.Date);
            }

            if (updatedDate.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.UpdatedDate.HasValue && bm.UpdatedDate.Value.Date == updatedDate.Value.Date);
            }

            if (isActive.HasValue)
            {
                boardMembersQuery = boardMembersQuery.Where(u => u.IsActive == isActive.Value);
            }

            List<BoardMemberDetail> lists = await boardMembersQuery
                .Select(bm => _mapper.Map<BoardMemberDetail>(bm))
                .ToListAsync();

            return lists;
        }

        public async Task<BoardMemberDetail> UpdateBoardMemberAsync(Guid id, Guid roleId)
        {

            var boardMember = await _unitOfWork.BoardMemberRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            boardMember.RoleId = roleId;
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
        public async Task<string> GetCurrentRoleAsync(Guid boardId)
        {
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var boardMember = await _unitOfWork.BoardMemberRepository.FirstOrDefaultAsync(bm => bm.UserId == currentUserId && bm.BoardId == boardId && bm.IsActive);

            if (boardMember == null)
            {
                throw new ExceptionResponse(HttpStatusCode.NotFound, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);
            }

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(boardMember.RoleId);

            return role.Name;
        }
        public async Task<BoardMember> GetBoardMemberByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.BoardMemberRepository.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
        }
    }
}
