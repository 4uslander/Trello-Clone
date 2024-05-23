using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Label
    {
        public Guid Id { get; set; }
        public Guid BoardId { get; set; }
        public string? Name { get; set; }
        public string Color { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Board Board { get; set; } = null!;
    }
}
