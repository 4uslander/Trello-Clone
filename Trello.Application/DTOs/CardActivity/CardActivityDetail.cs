using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.CardActivity
{
    public class CardActivityDetail
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid UserId { get; set; }
        public string Activity { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public Guid CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedUser { get; set; }
        public bool IsActive { get; set; }
    }
}
