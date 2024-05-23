using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
