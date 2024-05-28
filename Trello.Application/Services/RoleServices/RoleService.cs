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
using Trello.Application.DTOs.BoardMember;
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
        public List<RoleDetail> GetAllRole(string? name)
        {
            IQueryable<Role> rolesQuery = _unitOfWork.RoleRepository.GetAll();


            if (!string.IsNullOrEmpty(name))
            {
                rolesQuery = rolesQuery.Where(u => u.Name.Contains(name));
            }

            List<RoleDetail> lists = rolesQuery
                .Select(u => _mapper.Map<RoleDetail>(u))
                .ToList();

            return lists;
        }
        public async Task<RoleDetail> UpdateRoleAsync(Guid id, RoleDTO requestBody)
        {

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);


            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            role = _mapper.Map(requestBody, role);
            role.UpdatedDate = DateTime.Now;
            role.UpdatedUser = Guid.Parse(currentUserId);

            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            var roleDetail = _mapper.Map<RoleDetail>(role);
            return roleDetail;
        }
        public async Task<RoleDetail> ChangeStatusAsync(Guid Id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(Id);
            if (role == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
                throw new ExceptionResponse(HttpStatusCode.Unauthorized, ErrorField.AUTHENTICATION_FIELD, ErrorMessage.UNAUTHORIZED);

            role.UpdatedDate = DateTime.Now;
            role.UpdatedUser = Guid.Parse(currentUserId);

            if (role.IsActive == true)
            {
                role.IsActive = false;
            }
            else
            {
                role.IsActive = true;
            }

            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoardRole = _mapper.Map<RoleDetail>(role);
            return mappedBoardRole;
        }
        public async System.Threading.Tasks.Task IsExistRoleName(string? name)
        {
            var isExist = await _unitOfWork.RoleRepository.AnyAsync(x => x.Name.Equals(name));
            if (isExist)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_ALREADY_EXIST);
        }
    }
}
