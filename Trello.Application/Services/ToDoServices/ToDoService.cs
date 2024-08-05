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
using Trello.Application.DTOs.CardActivity;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.ToDo;
using Trello.Application.Services.BoardMemberServices;
using Trello.Application.Services.CardActivityServices;
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
        private readonly ICardActivityService _cardActivityService;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ICardService cardService, IBoardMemberService boardMemberService, ICardActivityService cardActivityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cardService = cardService;
            _boardMemberService = boardMemberService;
            _cardActivityService = cardActivityService;
        }

        public async Task<ToDoDetail> CreateToDoListAsync(CreateToDoDTO requestBody)
        {
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Check if the associated card exists
            var existingCard = await _cardService.GetCardByIdAsync(requestBody.CardId);
            if (existingCard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.CARD_FIELD, ErrorMessage.CARD_NOT_EXIST);
            }

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Check if the current user is a board member
            var existingBoardMember = await _boardMemberService.GetBoardMemberByUserIdAsync(currentUserId);
            if (existingBoardMember == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_MEMBER_FIELD, ErrorMessage.BOARD_MEMBER_NOT_EXIST);
            }

            // Map the request body to a ToDo entity and set its properties
            var todo = _mapper.Map<ToDo>(requestBody);
            todo.Id = Guid.NewGuid();
            todo.IsActive = true;
            todo.CreatedDate = DateTime.UtcNow;
            todo.CreatedUser = currentUserId;

            // Insert the new ToDo into the repository and save changes
            await _unitOfWork.ToDoRepository.InsertAsync(todo);

            var cardActivityRequest = new CreateCardActivityDTO
            {
                Activity = $"Add todo list {requestBody.Title} to this card",
                CardId = requestBody.CardId,
                UserId = todo.CreatedUser
            };

            await _cardActivityService.CreateCardActivityAsync(cardActivityRequest);

            await _unitOfWork.SaveChangesAsync();

            // Map the created ToDo to a ToDoDetail DTO and return it
            var createdToDoDto = _mapper.Map<ToDoDetail>(todo);
            return createdToDoDto;
        }

        public async Task<List<ToDoDetail>> GetAllToDoListAsync(Guid cardId)
        {
            // Get all active ToDo lists associated with the specified Card ID
            IQueryable<ToDo> todoListsQuery = _unitOfWork.ToDoRepository.GetAll();
            todoListsQuery = todoListsQuery.Where(u => u.CardId == cardId && u.IsActive);

            // Map the ToDo lists to ToDoDetail DTOs and return them
            List<ToDoDetail> todoLists = await todoListsQuery
                .Select(u => _mapper.Map<ToDoDetail>(u))
                .ToListAsync();

            return todoLists;
        }

        public async Task<List<ToDoDetail>> GetToDoListByFilterAsync(Guid cardId, string? title, bool? isActive)
        {
            // Get all ToDo lists associated with the specified Card ID
            IQueryable<ToDo> todoListsQuery = _unitOfWork.ToDoRepository.GetAll();
            todoListsQuery = todoListsQuery.Where(u => u.CardId == cardId);

            // Filter ToDo lists by title if provided
            if (!string.IsNullOrEmpty(title))
            {
                todoListsQuery = todoListsQuery.Where(u => u.Title.Contains(title));
            }

            // Filter ToDo lists by active status if provided
            if (isActive.HasValue)
            {
                todoListsQuery = todoListsQuery.Where(u => u.IsActive == isActive.Value);
            }

            // Map the ToDo lists to ToDoDetail DTOs and return them
            List<ToDoDetail> todoLists = await todoListsQuery
                .Select(u => _mapper.Map<ToDoDetail>(u))
                .ToListAsync();

            return todoLists;
        }

        public async Task<ToDoDetail> UpdateToDoListAsync(Guid id, ToDoDTO requestBody)
        {
            // Get the ToDo by ID and throw an exception if it doesn't exist
            var todo = await _unitOfWork.ToDoRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TODO_FIELD, ErrorMessage.TODO_NOT_EXIST);
            var preTodoTitle = todo.Title;

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the ToDo properties
            todo.UpdatedDate = DateTime.UtcNow;
            todo.UpdatedUser = currentUserId;
            todo.Title = requestBody.Title;

            // Update the ToDo in the repository and save changes
            _unitOfWork.ToDoRepository.Update(todo);


            var cardActivityRequest = new CreateCardActivityDTO 
            {
                Activity = $"Rename todo list  {requestBody.Title} from todo list {preTodoTitle} ",
                CardId = todo.CardId,
                UserId = todo.UpdatedUser
            };

            await _cardActivityService.CreateCardActivityAsync(cardActivityRequest);

            await _unitOfWork.SaveChangesAsync();

            // Map the updated ToDo to a ToDoDetail DTO and return it
            var todoDetail = _mapper.Map<ToDoDetail>(todo);
            return todoDetail;
        }

        public async Task<ToDoDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            // Get the ToDo by ID and throw an exception if it doesn't exist
            var todo = await _unitOfWork.ToDoRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.TODO_FIELD, ErrorMessage.TODO_NOT_EXIST);

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the ToDo's active status and metadata
            todo.UpdatedDate = DateTime.UtcNow;
            todo.UpdatedUser = currentUserId;
            todo.IsActive = isActive;

            // Update the ToDo in the repository and save changes
            _unitOfWork.ToDoRepository.Update(todo);



            var cardActivityRequest = new CreateCardActivityDTO
            {
                Activity = $"Removed todo list {todo.Title} from this card",
                CardId = todo.CardId,
                UserId = todo.UpdatedUser,
            };

            await _cardActivityService.CreateCardActivityAsync(cardActivityRequest);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated ToDo to a ToDoDetail DTO and return it
            var mappedList = _mapper.Map<ToDoDetail>(todo);
            return mappedList;
        }

        public async Task<ToDo> GetTodoListByIdAsync(Guid todoId)
        {
            // Get the ToDo by ID
            return await _unitOfWork.ToDoRepository.GetByIdAsync(todoId);
        }

    }
}