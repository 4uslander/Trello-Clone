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
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingList = await GetListByNameAsync(requestBody.Name, requestBody.BoardId);
            if (existingList != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_ALREADY_EXIST);
            }

            var existingBoard = await GetBoardByIdAsync(requestBody.BoardId);
            if (existingBoard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var latestPosition = await GetLatestListPositionAsync(requestBody.BoardId);

            var list = _mapper.Map<List>(requestBody);
            list.Id = Guid.NewGuid();
            list.IsActive = true;
            list.CreatedDate = DateTime.Now;
            list.CreatedUser = currentUserId;
            list.Position = latestPosition + 1;

            await _unitOfWork.ListRepository.InsertAsync(list);
            await _unitOfWork.SaveChangesAsync();

            var createdListDto = _mapper.Map<ListDetail>(list);
            return createdListDto;
        }
        public async Task<List<ListDetail>> GetAllListAsync(Guid boardId)
        {
            IQueryable<List> listsQuery = _unitOfWork.ListRepository.GetAll();

            listsQuery = listsQuery.Where(u => u.BoardId == boardId && u.IsActive);

            List<ListDetail> lists = await listsQuery
                .Select(u => _mapper.Map<ListDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<List<ListDetail>> GetListByFilterAsync(Guid boardId, string? name, bool? isActive)
        {
            IQueryable<List> listsQuery = _unitOfWork.ListRepository.GetAll();

            listsQuery = listsQuery.Where(l => l.BoardId == boardId);

            if (!string.IsNullOrEmpty(name))
            {
                listsQuery = listsQuery.Where(l => l.Name.Contains(name));
            }

            if (isActive.HasValue)
            {
                listsQuery = listsQuery.Where(l => l.IsActive == isActive.Value);
            }

            List<ListDetail> lists = await listsQuery
                .Select(l => _mapper.Map<ListDetail>(l))
                .ToListAsync();

            return lists;
        }

        public async Task<ListDetail> UpdateListNameAsync(Guid id, ListDTO requestBody)
        {
            var list = await _unitOfWork.ListRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            var existingList = await GetListByNameAsync(requestBody.Name, requestBody.BoardId);
            if (existingList != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_ALREADY_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            list = _mapper.Map(requestBody, list);
            list.UpdatedDate = DateTime.Now;
            list.UpdatedUser = currentUserId;

            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            var listDetail = _mapper.Map<ListDetail>(list);
            return listDetail;
        }

        public async Task<ListDetail> SwapListPositionsAsync(Guid id, Guid swappedListId)
        {
            var ListToSwap = await _unitOfWork.ListRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            var swappedList = await _unitOfWork.ListRepository.GetByIdAsync(swappedListId)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            if (ListToSwap.BoardId != swappedList.BoardId)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LISTS_IN_DIFFERENT_BOARD);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var tempPosition = ListToSwap.Position;

            // Swap the positions
            ListToSwap.Position = swappedList.Position;
            swappedList.Position = tempPosition;

            // Update the metadata
            ListToSwap.UpdatedDate = DateTime.Now;
            ListToSwap.UpdatedUser = currentUserId;
            swappedList.UpdatedDate = DateTime.Now;
            swappedList.UpdatedUser = currentUserId;

            // Update the lists in the repository
            _unitOfWork.ListRepository.Update(ListToSwap);
            _unitOfWork.ListRepository.Update(swappedList);

            await _unitOfWork.SaveChangesAsync();

            // Map the first list to ListDetail and return
            var listDetail = _mapper.Map<ListDetail>(ListToSwap);
            return listDetail;
        }

        public async Task<ListDetail> MoveListAsync(Guid id, int newPosition)
        {
            // Get the list to be moved
            var listToMove = await _unitOfWork.ListRepository.GetByIdAsync(id);
            if (listToMove == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            var currentPosition = listToMove.Position;

            if (currentPosition == newPosition)
                return _mapper.Map<ListDetail>(listToMove); 

            // Get all lists in the board
            var listsInBoard = await _unitOfWork.ListRepository
                .GetAll()
                .Where(x => x.BoardId == listToMove.BoardId && x.IsActive)
                .OrderBy(x => x.Position)
                .ToListAsync();

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
            var list = await _unitOfWork.ListRepository.GetByIdAsync(Id);
            if (list == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            list.UpdatedDate = DateTime.Now;
            list.UpdatedUser = currentUserId;
            list.IsActive = isActive;

            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<ListDetail>(list);
            return mappedList;
        }

        public async Task<List> GetListByNameAsync(string name, Guid boardId)
        {
            return await _unitOfWork.ListRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.BoardId == boardId);
        }
        public async Task<Board> GetBoardByIdAsync(Guid id)
        {
            return await _unitOfWork.BoardRepository.GetByIdAsync(id);
        }
        public async Task<int> GetLatestListPositionAsync(Guid boardId)
        {
            var latestPosition = await _unitOfWork.ListRepository.GetAll().Where(x => x.BoardId == boardId)
                .OrderByDescending(x => x.Position).Select(x => x.Position)
                .FirstOrDefaultAsync();

            return latestPosition;
        }

        public async Task<List> GetListByIdAsync(Guid id)
        {
            return await _unitOfWork.ListRepository.GetByIdAsync(id);
        }
    }
}
