using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.Notification;
using Trello.Application.Services.BoardServices;
using Trello.Application.Services.NotificationServices;
using Trello.Application.Services.RoleServices;
using Trello.Application.Services.UserServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.FirebaseNoti;
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
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly INotificationService _notificationService;

        public BoardMemberService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBoardService boardService,
            IUserService userService, IRoleService roleService, IFirebaseNotificationService firebaseNotificationService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _boardService = boardService;
            _userService = userService;
            _roleService = roleService;
            _firebaseNotificationService = firebaseNotificationService;
            _notificationService = notificationService;
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
            boardMember.CreatedDate = DateTime.UtcNow;
            boardMember.CreatedUser = currentUserId;
            boardMember.RoleId = memberRole.Id;
            boardMember.IsActive = true;

            await _unitOfWork.BoardMemberRepository.InsertAsync(boardMember);

            //
            var notificationRequest = new NotificationDTO
            {
                UserId = requestBody.UserId,
                Title = NotificationTitleField.INVITE_TO_BOARD,
                Body = $"\n{NotificationBodyField.INVITE_TO_BOARD}: {existingBoard.Name}."
            };

            var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);

            // Send notification
            await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);

            await _unitOfWork.SaveChangesAsync();
            var createdBoardMemberDto = _mapper.Map<BoardMemberDetail>(boardMember);

            return createdBoardMemberDto;
        } 

        public async Task<List<BoardMemberDetail>> GetAllBoardMemberAsync(Guid boardId)
        {
            IQueryable<BoardMember> boardMembersQuery = _unitOfWork.BoardMemberRepository.GetAll();

            boardMembersQuery = boardMembersQuery.Where(u => u.BoardId == boardId && u.IsActive);

            var adminRole = BoardMemberRoleEnum.Admin.ToString();

            List<BoardMemberDetail> lists = await boardMembersQuery
                .Select(bm => new BoardMemberDetail
                {
                    Id = bm.Id,
                    BoardId = bm.BoardId,
                    UserId = bm.UserId,
                    UserName = bm.User.Name,
                    UserEmail = bm.User.Email,
                    RoleId = bm.RoleId,
                    RoleName = bm.Role.Name,
                    IsActive = bm.IsActive,
                    CreatedDate = bm.CreatedDate,
                    UpdatedDate = bm.UpdatedDate
                })
                .OrderByDescending(bm => bm.RoleName == adminRole)
                .ThenBy(bm => bm.RoleName)
                .ToListAsync();

            return lists;
        }

        public async Task<List<BoardMemberDetail>> GetBoardMemberByFilterAsync(Guid boardId, string? name, bool? isActive)
        {
            IQueryable<BoardMember> boardMembersQuery = _unitOfWork.BoardMemberRepository.GetAll();

            boardMembersQuery = boardMembersQuery.Where(bm => bm.BoardId == boardId);

            if (!string.IsNullOrEmpty(name))
            {
                boardMembersQuery = boardMembersQuery.Where(bm => bm.User.Name.Contains(name));
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
            boardMember.UpdatedDate = DateTime.UtcNow;
            boardMember.UpdatedUser = currentUserId;

            _unitOfWork.BoardMemberRepository.Update(boardMember);

            //
            var existingUser = await _userService.GetUserIdByBoardMemberIdAsync(id);
            if (existingUser == Guid.Empty)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var notificationRequest = new NotificationDTO
            {
                UserId = existingUser,
                Title = NotificationTitleField.MEMBER_ROLE_UPDATED,
                Body = $"{NotificationTitleField.MEMBER_ROLE_UPDATED}"
            };

            var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);
 
            await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);

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

            boardMember.UpdatedDate = DateTime.UtcNow;
            boardMember.UpdatedUser = currentUserId;
            boardMember.IsActive = isActive;

            _unitOfWork.BoardMemberRepository.Update(boardMember);

            //
            var existingUser = await _userService.GetUserIdByBoardMemberIdAsync(Id);
            if (existingUser == Guid.Empty)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);
            }

            var notificationRequest = new NotificationDTO
            {
                UserId = existingUser,
                Title = NotificationTitleField.MEMBER_REMOVED,
                Body = $"\n{NotificationBodyField.MEMBER_REMOVED}"
            };

            var notificationDetail = await _notificationService.CreateNotificationAsync(notificationRequest);

            await _firebaseNotificationService.SendNotificationAsync(notificationDetail.UserId, notificationDetail.Title, notificationDetail.Body);

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
