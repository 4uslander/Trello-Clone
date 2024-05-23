using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class BoardMember
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Board Board { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
