using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Card;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.ToDo;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.CardMemberServices;
using Trello.Application.Services.CardServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.ToDoServices
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICardService _cardService;
        private readonly IBoardMemberService _boardMemberService;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICardService cardService, IBoardMemberService boardMemberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _boardMemberService = boardMemberService;
        }

        public async Task<ToDoDetail> CreateToDoListAsync(CreateToDoDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var existingBoardMember = await _boardMemberService.GetBoardMemberByUserIdAsync(currentUserId);
            if (existingBoardMember == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);
            }

            var todo = _mapper.Map<ToDo>(requestBody);
            todo.Id = Guid.NewGuid();
            todo.IsActive = true;
            todo.CreatedDate = DateTime.Now;
            todo.CreatedUser = currentUserId;

            await _unitOfWork.ToDoRepository.InsertAsync(todo);
            await _unitOfWork.SaveChangesAsync();

            var createdToDoDto = _mapper.Map<ToDoDetail>(todo);
            return createdToDoDto;
        }

        public async Task<List<ToDoDetail>> GetAllToDoListAsync(Guid cardId)
        {
            IQueryable<ToDo> todoListsQuery = _unitOfWork.ToDoRepository.GetAll();

            todoListsQuery = todoListsQuery.Where(u => u.CardId == cardId && u.IsActive);

            List<ToDoDetail> todoLists = await todoListsQuery
                .Select(u => _mapper.Map<ToDoDetail>(u))
                .ToListAsync();

            return todoLists;
        }

        public async Task<List<ToDoDetail>> GetToDoListByFilterAsync(Guid cardId, string? title, bool? isActive)
        {
            IQueryable<ToDo> todoListsQuery = _unitOfWork.ToDoRepository.GetAll();

            todoListsQuery = todoListsQuery.Where(u => u.CardId == cardId);

            if (!string.IsNullOrEmpty(title))
            {
                todoListsQuery = todoListsQuery.Where(u => u.Title.Contains(title));
            }
            if (isActive.HasValue)
            {
                todoListsQuery = todoListsQuery.Where(u => u.IsActive == isActive.Value);
            }

            List<ToDoDetail> todoLists = await todoListsQuery
                .Select(u => _mapper.Map<ToDoDetail>(u))
                .ToListAsync();

            return todoLists;
        }

        public async Task<ToDoDetail> UpdateToDoListAsync(Guid id, ToDoDTO requestBody)
        {
            var todo = await _unitOfWork.ToDoRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TODO_FIELD, ErrorMessage.TODO_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            todo.UpdatedDate = DateTime.UtcNow;
            todo.UpdatedUser = currentUserId;
            todo.Title = requestBody.Title;

            _unitOfWork.ToDoRepository.Update(todo);
            await _unitOfWork.SaveChangesAsync();

            var todoDetail = _mapper.Map<ToDoDetail>(todo);
            return todoDetail;
        }

        public async Task<ToDoDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            var todo = await _unitOfWork.ToDoRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TODO_FIELD, ErrorMessage.TODO_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            todo.UpdatedDate = DateTime.Now;
            todo.UpdatedUser = currentUserId;
            todo.IsActive = isActive;

            _unitOfWork.ToDoRepository.Update(todo);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<ToDoDetail>(todo);
            return mappedList;
        }

        public async Task<ToDo> GetTodoListByIdAsync(Guid todoId)
        {
            return await _unitOfWork.ToDoRepository.GetByIdAsync(todoId);
        }
    }
}