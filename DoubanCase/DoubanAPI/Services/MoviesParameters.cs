using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubanAPI.Services
{
    public class MoviesParameters
    {
        private int MaxPageSize { get; set; } = 100;
        public int TypeId { get; set; } = 1;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 50;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value>MaxPageSize)?MaxPageSize:value;
        }


    }
}
