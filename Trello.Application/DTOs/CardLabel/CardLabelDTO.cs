using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.CardLabel
{
    public class CardLabelDTO
    {
        [Required(ErrorMessage = "Card Id is required")]
        public Guid CardId { get; set; }

        [Required(ErrorMessage = "Label Id is required")]
        public Guid LabelId { get; set; }


    }
}
