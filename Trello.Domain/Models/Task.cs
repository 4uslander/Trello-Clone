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
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsChecked { get; set; }
        public bool IsActive { get; set; }

        public virtual ToDo Todo { get; set; } = null!;
    }
}
