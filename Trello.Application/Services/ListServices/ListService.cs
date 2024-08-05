using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Trello.Application.Services.BoardServices;

namespace Trello.Application.Services.ListServices
{
    public class ListService : IListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ListService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ListDetail> CreateListAsync(ListDTO requestBody)
        {
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Check if a list with the same name already exists in the specified board
            var existingList = await GetListByNameAsync(requestBody.Name, requestBody.BoardId);
            if (existingList != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_ALREADY_EXIST);
            }

            // Check if the specified board exists
            var existingBoard = await GetBoardByIdAsync(requestBody.BoardId);
            if (existingBoard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Get the latest position of the list in the specified board
            var latestPosition = await GetLatestListPositionAsync(requestBody.BoardId);

            // Map the request body to a List entity and set its properties
            var list = _mapper.Map<List>(requestBody);
            list.Id = Guid.NewGuid();
            list.IsActive = true;
            list.CreatedDate = DateTime.UtcNow;
            list.CreatedUser = currentUserId;
            list.Position = latestPosition + 1;

            // Insert the new list into the repository and save changes
            await _unitOfWork.ListRepository.InsertAsync(list);
            await _unitOfWork.SaveChangesAsync();

            // Map the created list to a ListDetail DTO and return it
            var createdListDto = _mapper.Map<ListDetail>(list);
            return createdListDto;
        }

        public async Task<List<ListDetail>> GetAllListAsync(Guid boardId)
        {
            // Get all lists for the specified board that are active
            IQueryable<List> listsQuery = _unitOfWork.ListRepository.GetAll();
            listsQuery = listsQuery.Where(u => u.BoardId == boardId && u.IsActive);

            // Map the lists to ListDetail DTOs and return them
            List<ListDetail> lists = await listsQuery
                .Select(u => _mapper.Map<ListDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<List<ListDetail>> GetListByFilterAsync(Guid boardId, string? name, bool? isActive)
        {
            // Get all lists for the specified board
            IQueryable<List> listsQuery = _unitOfWork.ListRepository.GetAll();
            listsQuery = listsQuery.Where(l => l.BoardId == boardId);

            // Filter lists by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                listsQuery = listsQuery.Where(l => l.Name.Contains(name));
            }

            // Filter lists by active status if provided
            if (isActive.HasValue)
            {
                listsQuery = listsQuery.Where(l => l.IsActive == isActive.Value);
            }

            // Map the lists to ListDetail DTOs and return them
            List<ListDetail> lists = await listsQuery
                .Select(l => _mapper.Map<ListDetail>(l))
                .ToListAsync();

            return lists;
        }

        public async Task<ListDetail> UpdateListNameAsync(Guid id, ListDTO requestBody)
        {
            // Get the list by ID and throw an exception if it doesn't exist
            var list = await _unitOfWork.ListRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            // Check if a list with the new name already exists in the specified board
            var existingList = await GetListByNameAsync(requestBody.Name, requestBody.BoardId);
            if (existingList != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_ALREADY_EXIST);
            }

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the list with the new details
            list = _mapper.Map(requestBody, list);
            list.UpdatedDate = DateTime.UtcNow;
            list.UpdatedUser = currentUserId;

            // Update the list in the repository and save changes
            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated list to a ListDetail DTO and return it
            var listDetail = _mapper.Map<ListDetail>(list);
            return listDetail;
        }

        public async Task<ListDetail> SwapListPositionsAsync(Guid id, Guid swappedListId)
        {
            // Get the lists to swap and throw an exception if they don't exist
            var ListToSwap = await _unitOfWork.ListRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);
            var swappedList = await _unitOfWork.ListRepository.GetByIdAsync(swappedListId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            // Check if the lists belong to the same board
            if (ListToSwap.BoardId != swappedList.BoardId)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LISTS_IN_DIFFERENT_BOARD);
            }

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Swap the positions of the lists
            var tempPosition = ListToSwap.Position;
            ListToSwap.Position = swappedList.Position;
            swappedList.Position = tempPosition;

            // Update the metadata for the lists
            ListToSwap.UpdatedDate = DateTime.UtcNow;
            ListToSwap.UpdatedUser = currentUserId;
            swappedList.UpdatedDate = DateTime.UtcNow;
            swappedList.UpdatedUser = currentUserId;

            // Update the lists in the repository and save changes
            _unitOfWork.ListRepository.Update(ListToSwap);
            _unitOfWork.ListRepository.Update(swappedList);
            await _unitOfWork.SaveChangesAsync();

            // Map the first list to ListDetail and return it
            var listDetail = _mapper.Map<ListDetail>(ListToSwap);
            return listDetail;
        }

        public async Task<ListDetail> MoveListAsync(Guid id, int newPosition)
        {
            // Get the list to be moved and throw an exception if it doesn't exist
            var listToMove = await _unitOfWork.ListRepository.GetByIdAsync(id);
            if (listToMove == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            // Check if the new position is the same as the current position
            var currentPosition = listToMove.Position;
            if (currentPosition == newPosition)
                return _mapper.Map<ListDetail>(listToMove);

            // Get all active lists in the board
            var listsInBoard = await _unitOfWork.ListRepository
                .GetAll()
                .Where(x => x.BoardId == listToMove.BoardId && x.IsActive)
                .OrderBy(x => x.Position)
                .ToListAsync();

            // Validate the new position
            if (newPosition < 1 || newPosition > listsInBoard.Count)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_POSITION_SPECIFIED);

            // Adjust positions of the other lists
            if (newPosition < currentPosition)
            {
                foreach (var list in listsInBoard.Where(x => x.Position >= newPosition && x.Position < currentPosition))
                {
                    list.Position++;
                    _unitOfWork.ListRepository.Update(list);
                }
            }
            else
            {
                foreach (var list in listsInBoard.Where(x => x.Position > currentPosition && x.Position <= newPosition))
                {
                    list.Position--;
                    _unitOfWork.ListRepository.Update(list);
                }
            }

            // Update the position of the list to be moved
            listToMove.Position = newPosition;
            _unitOfWork.ListRepository.Update(listToMove);

            // Save changes
            await _unitOfWork.SaveChangesAsync();

            // Return the updated list
            var listDetail = _mapper.Map<ListDetail>(listToMove);
            return listDetail;
        }

        public async Task<ListDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            // Get the list by ID and throw an exception if it doesn't exist
            var list = await _unitOfWork.ListRepository.GetByIdAsync(Id);
            if (list == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the status and metadata of the list
            list.UpdatedDate = DateTime.UtcNow;
            list.UpdatedUser = currentUserId;
            list.IsActive = isActive;

            // Update the list in the repository and save changes
            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated list to ListDetail and return it
            var mappedList = _mapper.Map<ListDetail>(list);
            return mappedList;
        }

        public async Task<List> GetListByNameAsync(string name, Guid boardId)
        {
            // Get the list by name and board ID
            return await _unitOfWork.ListRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.BoardId == boardId);
        }

        public async Task<Board> GetBoardByIdAsync(Guid id)
        {
            // Get the board by ID
            return await _unitOfWork.BoardRepository.GetByIdAsync(id);
        }

        public async Task<int> GetLatestListPositionAsync(Guid boardId)
        {
            // Get the latest position of the list in the specified board
            var latestPosition = await _unitOfWork.ListRepository.GetAll().Where(x => x.BoardId == boardId)
                .OrderByDescending(x => x.Position).Select(x => x.Position)
                .FirstOrDefaultAsync();

            return latestPosition;
        }

        public async Task<List> GetListByIdAsync(Guid id)
        {
            // Get the list by ID
            return await _unitOfWork.ListRepository.GetByIdAsync(id);
        }

    }
}
