using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.Board
{
    public class BoardDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedUser { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
    }
}
