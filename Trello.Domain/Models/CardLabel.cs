using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class CardLabel
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid LabelId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual Label Label { get; set; } = null!;
    }
}
