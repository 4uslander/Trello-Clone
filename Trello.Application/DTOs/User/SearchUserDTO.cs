﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.DTOs.User
{
    public class SearchUserDTO
    {
        public string? Email { get; set; }
        public string? Name { get; set; } 
        public string? Gender { get; set; }
    }
}