using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Helpers
{
    internal class DataWithPaging
    {
        public object Data { get; set; }
        public object Paging { get; set; }
        public DataWithPaging(object data, PageLinkBuilder paging)
        {
            this.Paging = new { First = paging.FirstPage, Last = paging.LastPage, Previous = paging.PreviousPage, Next = paging.PreviousPage };
            this.Data = data;
        }
    }
}
