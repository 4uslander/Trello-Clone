using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int IsActive { get; set; }
    }
}
