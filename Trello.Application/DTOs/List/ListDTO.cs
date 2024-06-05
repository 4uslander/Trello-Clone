using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.List
{
    public class ListDTO
    {
        [Required(ErrorMessage = "Board Id is required")]
        public Guid BoardId { get; set; }

        [Required(ErrorMessage = "List Name is required")]
        [MaxLength(150, ErrorMessage = "List Name cannot exceed 150 characters")]
        public string Name { get; set; } = null!;
    }
}
