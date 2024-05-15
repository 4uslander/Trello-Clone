using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Label
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string? Name { get; set; }
        public string Color { get; set; } = null!;
        public int IsActive { get; set; }

        public virtual Board Board { get; set; } = null!;
    }
}
