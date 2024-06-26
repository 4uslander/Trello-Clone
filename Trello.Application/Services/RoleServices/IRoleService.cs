using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.BoardMember;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.Role;
using Trello.Domain.Models;

namespace Trello.Application.Services.RoleServices
{
    public interface IRoleService
    {
        public Task<RoleDetail> CreateRoleAsync(RoleDTO requestBody);
        public Task<List<RoleDetail>> GetAllRoleAsync();
        public Task<List<RoleDetail>> GetRoleByFilterAsync(string? name, Guid? createdUser, Guid? updatedUser,
            DateTime? createdDate, DateTime? updatedDate, bool? isActive);
        public Task<RoleDetail> UpdateRoleAsync(Guid id, RoleDTO requestBody);
        public Task<RoleDetail> ChangeStatusAsync(Guid Id, bool isActive);
        public Task<Role> GetRoleByNameAsync(string name);
    }
}
