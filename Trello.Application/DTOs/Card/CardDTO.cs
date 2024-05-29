using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Card
{
    public class CardDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        public string Title { get; set; } = null!;
    }
    public class CreateCardDTO : CardDTO
    {
        [Required(ErrorMessage = "List Id is required")]
        public Guid ListId { get; set; }
    }
    public class UpdateCardDTO : CardDTO
    {
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReminderDate { get; set; }
    }
}
