using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.CardLabel
{
    public class CardLabelDetail
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid LabelId { get; set; }
        public string? LabelName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }
    }
}
