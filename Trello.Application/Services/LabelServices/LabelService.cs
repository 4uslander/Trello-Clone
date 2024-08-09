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
            // Validate the request body to ensure it's not null
            if (requestBody == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            }

            // Retrieve the board by its ID and check if it exists
            var existingBoard = await _boardService.GetBoardByIdAsync(requestBody.BoardId);
            if (existingBoard == null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.BOARD_FIELD, ErrorMessage.BOARD_NOT_EXIST);
            }

            // Get the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the request body to a Label entity and set additional properties
            var label = _mapper.Map<Label>(requestBody);
            label.Id = Guid.NewGuid();
            label.BoardId = requestBody.BoardId;
            label.CreatedDate = DateTime.Now;
            label.CreatedUser = currentUserId;
            label.IsActive = true;

            // Insert the new label into the repository and save changes
            await _unitOfWork.LabelRepository.InsertAsync(label);
            await _unitOfWork.SaveChangesAsync();

            // Map the created label entity to a DTO and return it
            var createLabelDto = _mapper.Map<LabelDetail>(label);
            return createLabelDto;
        }


        public async Task<List<LabelDetail>> GetAllLabelAsync(Guid BoardId)
        {
            // Get all labels for the specified board that are active
            IQueryable<Label> labelsQuery = _unitOfWork.LabelRepository.GetAll();
            labelsQuery = labelsQuery.Where(u => u.BoardId == BoardId && u.IsActive);

            // Map the label entities to DTOs and return them as a list
            List<LabelDetail> labels = await labelsQuery.Select(u => _mapper.Map<LabelDetail>(u)).ToListAsync();
            return labels;
        }


        public async Task<List<LabelDetail>> GetLabelByFilterAsync(Guid BoardId, string? Name, string? Color, bool? isActive)
        {
            // Get all labels for the specified board
            IQueryable<Label> labelsQuery = _unitOfWork.LabelRepository.GetAll();
            labelsQuery = labelsQuery.Where(c => c.BoardId == BoardId);

            // Apply filters if provided
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

            // Map the filtered label entities to DTOs and return them as a list
            List<LabelDetail> labels = await labelsQuery.Select(c => _mapper.Map<LabelDetail>(c)).ToListAsync();
            return labels;
        }

        public async Task<LabelDetail> UpdateLabelAsync(Guid id, UpdateLabelDTO requestBody)
        {
            // Retrieve the label by its ID and throw an error if it doesn't exist
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LABEL_FIELD, ErrorMessage.LABEL_NOT_EXIST);

            // Get the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the updated properties from the request body to the label entity
            label = _mapper.Map(requestBody, label);
            label.UpdatedDate = DateTime.Now;
            label.UpdatedUser = currentUserId;

            // Update the label in the repository and save changes
            _unitOfWork.LabelRepository.Update(label);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated label entity to a DTO and return it
            var labelDetail = _mapper.Map<LabelDetail>(label);
            return labelDetail;
        }


        public async Task<LabelDetail> ChangeStatusAsync(Guid Id, bool IsActive)
        {
            // Retrieve the label by its ID and throw an error if it doesn't exist
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(Id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.LABEL_FIELD, ErrorMessage.LABEL_NOT_EXIST);

            // Get the current user's ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the label's status and the current user's ID
            label.UpdatedDate = DateTime.Now;
            label.UpdatedUser = currentUserId;
            label.IsActive = IsActive;

            // Update the label in the repository and save changes
            _unitOfWork.LabelRepository.Update(label);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated label entity to a DTO and return it
            var mappedList = _mapper.Map<LabelDetail>(label);
            return mappedList;
        }

        public async Task<Label> GetLabelByIdAsync(Guid Id)
        {
            // Retrieve the label by its ID and return it
            return await _unitOfWork.LabelRepository.GetByIdAsync(Id);
        }


    }
}
