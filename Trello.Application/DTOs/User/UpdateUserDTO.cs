using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(50, ErrorMessage = "Gender cannot exceed 50 characters")]
        public string Gender { get; set; } = null!;
    }
}
