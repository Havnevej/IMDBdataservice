using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBdataservice
{
    //credit, course example
    public class QueryStringOur
    {
        private int _pageSize = 10;

        public const int MaxPageSize = 500;

        public int Page { get; set; } = 0;
        public string needle { get; set;}
        public string username { get; set; }


        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string personId { get; set; }
        public string OrderBy { get; set; }
        public string Genre { get; set; }
        public string PrimaryTitle { get; set; }
        public string PersonName { get; set; }

        public string titleId { get; set; }

    }
}