using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.Role;

namespace Trello.Application.Services.RoleServices
{
    public interface IRoleService
    {
        public Task<RoleDetail> CreateRoleAsync(RoleDTO requestBody);
        public List<RoleDetail> GetAllRole(string? name);
        public Task<RoleDetail> UpdateRoleAsync(Guid id, RoleDTO requestBody);
        public Task<RoleDetail> ChangeStatusAsync(Guid Id);
        public Task IsExistRoleName(string? name);
    }
}
