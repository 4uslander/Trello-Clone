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
using Trello.Application.DTOs.List;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Domain.Enums;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

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

            await IsExistListName(requestBody.Name, requestBody.BoardId);
            await IsExistBoardId(requestBody.BoardId);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);
            var latestPosition = await GetLatestListPositionAsync(requestBody.BoardId);

            var list = _mapper.Map<List>(requestBody);
            list.Id = Guid.NewGuid();
            list.IsActive = true;
            list.CreatedDate = DateTime.Now;
            list.CreatedUser = Guid.Parse(currentUserId);
            list.Position = latestPosition + 1;
            

            await _unitOfWork.ListRepository.InsertAsync(list);
            await _unitOfWork.SaveChangesAsync();

            var createdListDto = _mapper.Map<ListDetail>(list);
            return createdListDto;
        }
        public List<ListDetail> GetAllList(string? name)
        {
            IQueryable<List> listsQuery = _unitOfWork.ListRepository.GetAll();


            if (!string.IsNullOrEmpty(name))
            {
                listsQuery = listsQuery.Where(u => u.Name.Contains(name));
            }

            List<ListDetail> lists = listsQuery
                .Select(u => _mapper.Map<ListDetail>(u))
                .ToList();

            return lists;
        }
        public async Task<ListDetail> UpdateListAsync(Guid id, ListDTO requestBody)
        {

            var list = await _unitOfWork.ListRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            await IsExistListName(requestBody.Name, requestBody.BoardId);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            list = _mapper.Map(requestBody, list);
            list.UpdatedDate = DateTime.Now;
            list.UpdatedUser = Guid.Parse(currentUserId);

            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            var listDetail = _mapper.Map<ListDetail>(list);
            return listDetail;
        }

        public async Task<ListDetail> ChangeStatusAsync(Guid Id)
        {
            var list = await _unitOfWork.ListRepository.GetByIdAsync(Id);
            if (list == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            list.UpdatedDate = DateTime.Now;
            list.UpdatedUser = Guid.Parse(currentUserId);

            if (list.IsActive == true)
            {
                list.IsActive = false;
            }
            else
            {
                list.IsActive = true;
            }

            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<ListDetail>(list);
            return mappedList;
        }

        public async System.Threading.Tasks.Task IsExistListName(string? name, Guid boardId)
        {
            var isExist = await _unitOfWork.ListRepository.AnyAsync(x => x.Name == name && x.BoardId == boardId);
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_ALREADY_EXIST);
        }
        public async System.Threading.Tasks.Task IsExistBoardId(Guid? id)
        {
            var boardExists = await _unitOfWork.BoardRepository.AnyAsync(x => x.Id.Equals(id));
            if (!boardExists)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
        }
        public async Task<int> GetLatestListPositionAsync(Guid boardId)
        {
            var latestPosition = await _unitOfWork.ListRepository.GetAll().Where(x => x.BoardId == boardId)
                .OrderByDescending(x => x.Position).Select(x => x.Position)
                .FirstOrDefaultAsync();

            return latestPosition;
        }
    }
}