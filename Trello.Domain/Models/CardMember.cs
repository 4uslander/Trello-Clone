using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class CardMember
    {
        public int CardId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsActive { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
