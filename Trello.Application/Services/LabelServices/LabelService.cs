using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Label;
using Trello.Application.Services.BoardServices;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.LabelServices
{
    public class LabelService : ILabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBoardService _boardService;

        public LabelService( IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor ,IBoardService boardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _boardService = boardService;
        }


        public async Task<LabelDetail> CreateLabelAsync(CreateLabelDTO requestBody)
        {
            if (requestBody == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }

            var existingBoard = await _boardService.GetBoardByIdAsync(requestBody.BoardId);
            if (existingBoard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

           var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var label = _mapper.Map<Label>(requestBody);
            label.Id = Guid.NewGuid();
            label.BoardId = requestBody.BoardId;
            label.CreatedDate = DateTime.Now;
            label.CreatedUser = currentUserId;
            label.IsActive = true;

            await _unitOfWork.LabelRepository.InsertAsync(label);
            await _unitOfWork.SaveChangesAsync();

            var createLabelDto = _mapper.Map<LabelDetail>(label);
            return createLabelDto;

        }


        public async Task<List<LabelDetail>> GetAllLabelAsync(Guid BoardId)
        {
            IQueryable<Label> labelsQuery = _unitOfWork.LabelRepository.GetAll();
            labelsQuery = labelsQuery.Where(u => u.BoardId == BoardId && u.IsActive);

            List<LabelDetail> labels = await labelsQuery.Select(u => _mapper.Map<LabelDetail>(u)).ToListAsync();
            return labels;
        }
   

        public async Task<List<LabelDetail>> GetLabelByFilterAsync(Guid BoardId, string? Name, string? Color, bool? isActive)
        {
            IQueryable<Label> labelsQuery = _unitOfWork.LabelRepository.GetAll();
            labelsQuery = labelsQuery.Where(c => c.BoardId == BoardId);
            
            if (!string.IsNullOrEmpty(Name))
            {
                labelsQuery = labelsQuery.Where(c => c.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(Color))
            {
                labelsQuery = labelsQuery.Where(x => x.Color.Contains(Color));
            }

            if (isActive.HasValue)
            {
                labelsQuery = labelsQuery.Where(c => c.IsActive == isActive.Value);
            }
            List<LabelDetail> labels = await labelsQuery.Select(c => _mapper.Map<LabelDetail>(c)).ToListAsync();
            return labels;
        }

        public async Task<LabelDetail> UpdateLabelAsync(Guid id, UpdateLabelDTO requestBody)
        {
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LABEL_FIELD, ErrorMessage.LABEL_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);



            label = _mapper.Map(requestBody, label);

            label.UpdatedDate = DateTime.Now;
            label.UpdatedUser = currentUserId;

            _unitOfWork.LabelRepository.Update(label);
            await _unitOfWork.SaveChangesAsync();

            var labelDetail = _mapper.Map<LabelDetail>(label);
            return labelDetail;
        }


        public async Task<LabelDetail> ChangeStatusAsync(Guid Id, bool IsActive)
        {
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LABEL_FIELD, ErrorMessage.LABEL_NOT_EXIST);
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);
            label.UpdatedDate = DateTime.Now;
            label.UpdatedUser = currentUserId;
            label.IsActive= IsActive;
            _unitOfWork.LabelRepository.Update(label);
            await _unitOfWork.SaveChangesAsync();
            var mappedList = _mapper.Map<LabelDetail>(label);
            return mappedList;

        }

        public async Task<Label> GetLabelByIdAsync(Guid Id)
        {
            return await _unitOfWork.LabelRepository.GetByIdAsync(Id);
        }
       
    }
}
