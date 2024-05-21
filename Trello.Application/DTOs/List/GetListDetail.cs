using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.List
{
    public class GetListDetail
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public string IsActiveString { get; set; }
    }
}
