using Microsoft.AspNetCore.Mvc;
using System;
using System.Web.Http.Routing;

namespace WebAPI.Controllers
{
    internal class PageLinkBuilder
    {


        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PageLinkBuilder(IUrlHelper url, string route, object extraValues, int pageNum, int pageSize, long total)
        {
            Console.WriteLine(total);
            Console.WriteLine(pageNum);
            // Determine total number of pages
            var pageCount = total > 0
                ? (int)Math.Ceiling(total / (double)pageSize)
                : 0;
 
            FirstPage = new Uri(url.Link(route, new HttpRouteValueDictionary(extraValues)
            {
                {"page", 1},
                {"pagesize", pageSize}
            }));
            LastPage = new Uri(url.Link(route, new HttpRouteValueDictionary(extraValues)
            {
                {"page", pageCount},
                {"pagesize", pageSize}
            }));
            if (pageNum > 1)
            {
                PreviousPage = new Uri(url.Link(route, new HttpRouteValueDictionary(extraValues)
                {
                    {"page", pageNum - 1},
                    {"pagesize", pageSize}
                }));
            }
            if (pageNum < pageCount)
            {
                NextPage = new Uri(url.Link(route, new HttpRouteValueDictionary(extraValues)
                {
                    {"page", pageNum + 1},
                    {"pagesize", pageSize}
                }));
            }
        }
    }
}