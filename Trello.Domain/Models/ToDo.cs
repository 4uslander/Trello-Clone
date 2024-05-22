using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class ToDo
    {
        public ToDo()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int CardId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Card Card { get; set; } = null!;
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
