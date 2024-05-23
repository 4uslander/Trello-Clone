using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Task
    {
        public Guid Id { get; set; }
        public Guid TodoId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsChecked { get; set; }
        public bool IsActive { get; set; }

        public virtual ToDo Todo { get; set; } = null!;
    }
}
