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
            if (requestBody == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.REQUEST_BODY, ErrorMessage.NULL_REQUEST_BODY);

            var existingRoleName = await GetRoleByNameAsync(requestBody.Name);
            if (existingRoleName != null)
            {
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_ALREADY_EXIST);
            }

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            var role = _mapper.Map<Role>(requestBody);
            role.Id = Guid.NewGuid();
            role.CreatedDate = DateTime.Now;
            role.CreatedUser = currentUserId;
            role.IsActive = true;

            await _unitOfWork.RoleRepository.InsertAsync(role);
            await _unitOfWork.SaveChangesAsync();

            var createdRoleDto = _mapper.Map<RoleDetail>(role);

            return createdRoleDto;
        }
        public async Task<List<RoleDetail>> GetAllRoleAsync()
        {
            IQueryable<Role> rolesQuery = _unitOfWork.RoleRepository.GetAll();

            List<RoleDetail> lists = await rolesQuery
                .Select(u => _mapper.Map<RoleDetail>(u))
                .ToListAsync();

            return lists;
        }

        public async Task<List<RoleDetail>> GetRoleByFilterAsync(string? name, bool? isActive)
        {
            IQueryable<Role> rolesQuery = _unitOfWork.RoleRepository.GetAll();

            if (!string.IsNullOrEmpty(name))
            {
                rolesQuery = rolesQuery.Where(r => r.Name.Contains(name));
            }

            if (isActive.HasValue)
            {
                rolesQuery = rolesQuery.Where(r => r.IsActive == isActive.Value);
            }

            List<RoleDetail> lists = await rolesQuery
                .Select(r => _mapper.Map<RoleDetail>(r))
                .ToListAsync();

            return lists;
        }

        public async Task<RoleDetail> UpdateRoleAsync(Guid id, RoleDTO requestBody)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id)
                ?? throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);


            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            role = _mapper.Map(requestBody, role);
            role.UpdatedDate = DateTime.Now;
            role.UpdatedUser = currentUserId;

            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            var roleDetail = _mapper.Map<RoleDetail>(role);
            return roleDetail;
        }
        public async Task<RoleDetail> ChangeStatusAsync(Guid Id, bool isActive)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(Id);
            if (role == null)
                throw new ExceptionResponse(HttpStatusCode.BadRequest, ErrorField.ROLE_FIELD, ErrorMessage.ROLE_NOT_EXIST);

            var currentUserId = UserAuthorizationHelper.GetUserAuthorizationById(_httpContextAccessor.HttpContext);

            role.UpdatedDate = DateTime.Now;
            role.UpdatedUser = currentUserId;
            role.IsActive = isActive;

            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            var mappedBoardRole = _mapper.Map<RoleDetail>(role);
            return mappedBoardRole;
        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            return await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
        }
    }
}
