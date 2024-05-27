using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Role
{
    public class RoleDTO
    {
        [Required(ErrorMessage = "Role Id is required")]
        public Guid CreatedUserId { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; } = null!;
    }
}
