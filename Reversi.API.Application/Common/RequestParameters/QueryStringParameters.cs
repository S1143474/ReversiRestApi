using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Reversi.API.Application.Common.RequestParameters
{
    public abstract class QueryStringParameters
    {
        private const int _MaxPageSize = 12;

        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers are allowed")]
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _MaxPageSize) ? _MaxPageSize : value;
        }

        public string OrderBy { get; set; }
    }
}
