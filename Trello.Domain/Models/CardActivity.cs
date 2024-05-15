using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class CardActivity
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string Activity { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
