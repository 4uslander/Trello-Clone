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

        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsActive { get; set; }

        public virtual Board Board { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; }
    }
}
