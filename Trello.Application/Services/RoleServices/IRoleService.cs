using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Application.DTOs.List;
using Trello.Application.DTOs.Role;

namespace Trello.Application.Services.RoleServices
{
    public interface IRoleService
    {
        public Task<RoleDetail> CreateRoleAsync(RoleDTO requestBody);
        public Task IsExistRoleName(string? name);
    }
}
