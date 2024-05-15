using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int IsActive { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
