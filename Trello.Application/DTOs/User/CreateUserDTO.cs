using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.[\w\.-]+$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(50, ErrorMessage = "Gender cannot exceed 50 characters")]
        public string Gender { get; set; } = null!;  
    }
}
