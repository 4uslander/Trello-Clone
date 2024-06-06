using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.ToDo
{
    public class ToDoDTO
    {
        [Required(ErrorMessage = "Card Id is required")]
        public Guid CardId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        public string Title { get; set; } = null!;
    }
}
