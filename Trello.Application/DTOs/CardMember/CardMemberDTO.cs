using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.CardMember
{
    public class CardMemberDTO
    {
        [Required(ErrorMessage = "Card Id is required")]
        public Guid CardId { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public Guid UserId { get; set; }
    }
}
