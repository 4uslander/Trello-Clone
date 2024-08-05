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
using Trello.Application.DTOs.Board;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.Role;
using Trello.Application.Utilities.ErrorHandler;
using Trello.Application.Utilities.Helper.GetUserAuthorization;
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
            // Check if the request body is null and throw an exception if it is
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            // Check if a role with the same name already exists
            var existingRoleName = await GetRoleByNameAsync(requestBody.Name);
            if (existingRoleName != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_ALREADY_EXIST);
            }

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the request body to a Role entity and set its properties
            var role = _mapper.Map<Role>(requestBody);
            role.Id = Guid.NewGuid();
            role.CreatedDate = DateTime.UtcNow;
            role.CreatedUser = currentUserId;
            role.IsActive = true;

            // Insert the new role into the repository and save changes
            await _unitOfWork.RoleRepository.InsertAsync(role);
            await _unitOfWork.SaveChangesAsync();

            // Map the created role to a RoleDetail DTO and return it
            var createdRoleDto = _mapper.Map<RoleDetail>(role);
            return createdRoleDto;
        }

        public async Task<List<RoleDetail>> GetAllRoleAsync()
        {
            // Get all roles from the repository
            IQueryable<Role> rolesQuery = _unitOfWork.RoleRepository.GetAll();

            // Map the roles to RoleDetail DTOs and return them
            List<RoleDetail> lists = await rolesQuery
                .Select(u => _mapper.Map<RoleDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<List<RoleDetail>> GetRoleByFilterAsync(string? name, bool? isActive)
        {
            // Get all roles from the repository
            IQueryable<Role> rolesQuery = _unitOfWork.RoleRepository.GetAll();

            // Filter roles by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                rolesQuery = rolesQuery.Where(r => r.Name.Contains(name));
            }

            // Filter roles by active status if provided
            if (isActive.HasValue)
            {
                rolesQuery = rolesQuery.Where(r => r.IsActive == isActive.Value);
            }

            // Map the roles to RoleDetail DTOs and return them
            List<RoleDetail> lists = await rolesQuery
                .Select(r => _mapper.Map<RoleDetail>(r))
                .ToListAsync();

            return lists;
        }

        public async Task<RoleDetail> UpdateRoleAsync(Guid id, RoleDTO requestBody)
        {
            // Get the role by ID and throw an exception if it doesn't exist
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Map the updated data to the existing role and set updated properties
            role = _mapper.Map(requestBody, role);
            role.UpdatedDate = DateTime.UtcNow;
            role.UpdatedUser = currentUserId;

            // Update the role in the repository and save changes
            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated role to a RoleDetail DTO and return it
            var roleDetail = _mapper.Map<RoleDetail>(role);
            return roleDetail;
        }

        public async Task<RoleDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            // Get the role by ID and throw an exception if it doesn't exist
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(Id);
            if (role == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);

            // Get the current user ID from the HTTP context
            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            // Update the status and metadata of the role
            role.UpdatedDate = DateTime.UtcNow;
            role.UpdatedUser = currentUserId;
            role.IsActive = isActive;

            // Update the role in the repository and save changes
            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            // Map the updated role to a RoleDetail DTO and return it
            var mappedBoardRole = _mapper.Map<RoleDetail>(role);
            return mappedBoardRole;
        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            // Get the role by name, ignoring case sensitivity
            return await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }

    }
}
