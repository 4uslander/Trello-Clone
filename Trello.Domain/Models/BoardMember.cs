using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class BoardMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BoardId { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsActive { get; set; }

        public virtual Board Board { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
