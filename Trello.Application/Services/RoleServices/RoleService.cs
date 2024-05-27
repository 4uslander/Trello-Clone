using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.Role;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;
using static Trello.Application.Utilities.GlobalVariables.GlobalVariable;

namespace Trello.Application.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<RoleDetail> CreateRoleAsync(RoleDTO requestBody)
        {
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);
            await IsExistRoleName(requestBody.Name);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(requestBody.CreatedUserId);
            if (user == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.USER_FIELD, ErrorMessage.USER_NOT_EXIST);

            var role = _mapper.Map<Role>(requestBody);
            role.Id = Guid.NewGuid();
            role.CreatedDate = DateTime.Now;
            role.CreatedUser = user.Id;
            role.IsActive = true;

            await _unitOfWork.RoleRepository.InsertAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var createdRoleDto = _mapper.Map<RoleDetail>(role);

            return createdRoleDto;
        }
        public async System.Threading.Tasks.Task IsExistRoleName(string? name)
        {
            var isExist = await _unitOfWork.RoleRepository.AnyAsync(x => x.Name.Equals(name));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_ALREADY_EXIST);
        }
    }
}
