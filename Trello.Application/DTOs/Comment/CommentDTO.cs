using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Comment
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "Card Id is required")]
        public Guid CardId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = null!;
    }
}
