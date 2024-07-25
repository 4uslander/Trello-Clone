using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.CardActivity
{
    public class CardActivityDTO
    {
        [Required(ErrorMessage = "Card Id is required")]
        public Guid CardId { get; set; }
        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; }
    }

    public class CreateCardActivityDTO : CardActivityDTO
    {
        [Required(ErrorMessage = "Activity is required")]
        public string Activity { get; set; } = null!;
    }
}
