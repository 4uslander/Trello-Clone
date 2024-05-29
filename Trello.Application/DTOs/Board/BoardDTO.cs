using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Board
{
    public class BoardDTO
    {
        [Required(ErrorMessage = "Board Name is required")]
        [MaxLength(50, ErrorMessage = "Board Name cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

    }
}
