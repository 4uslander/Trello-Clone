using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Label
{
    public class LabelDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = null!;

        [MaxLength(150, ErrorMessage = "Color cannot exceed 150 characters")]
        public string Color { get; set; } = null!;
    }

    public class CreateLabelDTO: LabelDTO
    {
        [Required(ErrorMessage = "BoardId is required")]
        public Guid BoardId { get; set; }
    }

    public class UpdateLabelDTO: LabelDTO
    {   public string? Name { get; set; }
        public string? Color { get; set; }   
    }


}
