using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class List
    {
        public List()
        {
            Cards = new HashSet<Card>();
        }

        public Guid Id { get; set; }
        public Guid BoardId { get; set; }
        public string? Name { get; set; }
        public int Position { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Board Board { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; }
    }
}
