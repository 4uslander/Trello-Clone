using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public int TodoId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int IsChecked { get; set; }
        public int IsActive { get; set; }

        public virtual ToDo Todo { get; set; } = null!;
    }
}
