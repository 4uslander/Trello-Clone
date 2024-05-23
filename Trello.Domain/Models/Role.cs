using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }
    }
}
