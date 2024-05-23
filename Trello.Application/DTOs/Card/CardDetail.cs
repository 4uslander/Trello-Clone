using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Card
{
    public class CardDetail
    {
        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsActive { get; set; }
    }
}
