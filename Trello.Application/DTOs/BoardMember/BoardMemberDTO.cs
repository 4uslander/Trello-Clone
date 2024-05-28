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
        [Required(ErrorMessage = "Role Id is required")]
        public Guid RoleId { get; set; }
    }
    public class CreateBoardMemberDTO : BoardMemberDTO
    {
        [Required(ErrorMessage = "User Id is required")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Board Id is required")]
        public Guid BoardId { get; set; }
        [Required(ErrorMessage = "Created user id is required")]
        public Guid CreatedUserId { get; set; }
    }
}
