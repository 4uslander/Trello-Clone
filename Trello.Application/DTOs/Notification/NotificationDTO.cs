using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Notification
{
    public class NotificationDTO
    {
        [Required(ErrorMessage = "User Id is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Message Title is required")]
        [MaxLength(255, ErrorMessage = "Message Title cannot exceed 255 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Message Body is required")]
        [MaxLength(255, ErrorMessage = "Message Body cannot exceed 255 characters")]
        public string Body { get; set; } = null!;
    }
}
