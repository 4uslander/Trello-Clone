using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Domain.Enums;

namespace Trello.Application.DTOs.User
{
    public class UserDTO
    {

    }
    public class CreateUserDTO : UserDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.[\w\.-]+$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters")]
        [MinLength(6, ErrorMessage = "Password must be more than 6 characters")]
        public string Password { get; set; } = null!;
    }
    public class UpdateUserDTO : UserDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [EnumDataType(typeof(GenderEnum))]
        public string Gender { get; set; } = null!;
    }
    public class UserLoginDTO : UserDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
