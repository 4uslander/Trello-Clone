using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.BoardMember
{
    public class BoardMemberDTO
    {
        [Required(ErrorMessage = "User Id is required")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Board Id is required")]
        public Guid BoardId { get; set; }
    }
}
