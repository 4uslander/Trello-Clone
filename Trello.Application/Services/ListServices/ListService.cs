using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public ListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<GetListDetail> CreateListAsync(CreateListDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            await IsExistListName(requestBody.Name);
            await IsExistBoardId(requestBody.BoardId);
            ValidateListPosition(requestBody.Position);
            await IsUniqueListPosition(requestBody.BoardId, requestBody.Position);

            var list = _mapper.Map<List>(requestBody);
            list.IsActive = (int)ListEnum.Active;

            await _unitOfWork.ListRepository.InsertAsync(list);
            await _unitOfWork.SaveChangesAsync();

            var createdListDto = _mapper.Map<GetListDetail>(list);
            return createdListDto;
        }
        public List<GetListDetail> GetAllList(string? name)
        {
            IQueryable<List> listsQuery = _unitOfWork.ListRepository.GetAll();


            if (!string.IsNullOrEmpty(name))
            {
                listsQuery = listsQuery.Where(u => u.Name.Contains(name));
            }

            List<GetListDetail> lists = listsQuery
                .Select(u => _mapper.Map<GetListDetail>(u))
                .ToList();

            return lists;
        }
        public async Task<GetListDetail> UpdateListAsync(int id, UpdateListDTO requestBody)
        {

            var list = await _unitOfWork.ListRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            await IsExistListName(requestBody.Name);
            ValidateListPosition(requestBody.Position);
            await IsUniqueListPosition(requestBody.BoardId, requestBody.Position);

            list = _mapper.Map(requestBody, list);

            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            var listDetail = _mapper.Map<GetListDetail>(list);
            return listDetail;
        }

        public async Task<GetListDetail> ChangeStatusAsync(int Id)
        {
            var list = await _unitOfWork.ListRepository.GetByIdAsync(Id);
            if (list == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_NOT_EXIST);

            if (list.IsActive == (int)UserStatus.Active)
            {
                list.IsActive = (int)UserStatus.InActive;
            }
            else
            {
                list.IsActive = (int)UserStatus.Active;
            }

            _unitOfWork.ListRepository.Update(list);
            await _unitOfWork.SaveChangesAsync();

            var mappedList = _mapper.Map<GetListDetail>(list);
            return mappedList;
        }

        public async System.Threading.Tasks.Task IsExistListName(string? name)
        {
            var isExist = await _unitOfWork.ListRepository.AnyAsync(x => x.Name.Equals(name));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_ALREADY_EXIST);
        }
        public async System.Threading.Tasks.Task IsExistBoardId(int? id)
        {
            var boardExists = await _unitOfWork.BoardRepository.AnyAsync(x => x.Id.Equals(id));
            if (!boardExists)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
        }

        public async System.Threading.Tasks.Task IsUniqueListPosition(int boardId, int position)
        {
            var isExist = await _unitOfWork.ListRepository.AnyAsync(x => x.BoardId == boardId && x.Position == position);
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.LIST_POSITION_ALREADY_EXIST);
        }

        private void ValidateListPosition(int position)
        {
            if (position < 1)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LIST_FIELD, ErrorMessage.INVALID_LIST_POSITION);
        }
    }
}
