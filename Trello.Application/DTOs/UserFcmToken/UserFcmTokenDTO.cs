using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.UserFcmToken
{
    public class UserFcmTokenDTO
    {
        [Required(ErrorMessage = "Fcm Token is required")]
        public string FcmToken { get; set; } = null!;
    }
    public class CreateUserFcmTokenDTO : UserFcmTokenDTO
    {
        [Required(ErrorMessage = "User Id is required")]
        public Guid UserId { get; set; }

    }
}
