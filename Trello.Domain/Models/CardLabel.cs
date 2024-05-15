using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class CardLabel
    {
        public int CardId { get; set; }
        public int LabelId { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual Label Label { get; set; } = null!;
    }
}
