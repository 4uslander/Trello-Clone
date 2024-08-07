﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.ResponseHandler
{
    public class ResponseModel
    {
        public class ApiResponse<T>
        {
            public int Code { get; set; }
            public T Data { get; set; }

        }
        public class LoginResponse<T>
        {
            public int Code { get; set; }
            public T Bearer { get; set; }

        }

        public class PagedApiResponse<T>
        {
            public int Code { get; set; }
            public PaginationInfo Paging { get; set; }
            public List<T> Data { get; set; }
            public int TotalCount { get; set; }
        }

        public class PagedApiResponseSpecificData<T>
        {
            public int Code { get; set; }
            public PaginationInfo Paging { get; set; }
            public T Data { get; set; }
        }


        public class PaginationInfo
        {
            public int Page { get; set; }
            public int Size { get; set; }
        }

    }
}
