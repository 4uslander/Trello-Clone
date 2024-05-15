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
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = null!;  
    }
}
