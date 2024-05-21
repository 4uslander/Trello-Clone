using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.List
{
    public class CreateListDTO
    {
        [Required(ErrorMessage = "Board Id is required")]
        public int BoardId { get; set; }

        [Required(ErrorMessage = "List Name is required")]
        [MaxLength(50, ErrorMessage = "Board Name cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "List Position is required")]
        public int Position { get; set; }
    }
}
