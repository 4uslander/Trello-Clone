using System;
using System.Collections.Generic;

namespace Trello.Domain.Models
{
    public partial class Board
    {
        public Board()
        {
            Labels = new HashSet<Label>();
            Lists = new HashSet<List>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int IsPublic { get; set; }
        public int IsActive { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
        public virtual ICollection<List> Lists { get; set; }
    }
}
