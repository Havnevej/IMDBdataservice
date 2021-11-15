using Microsoft.AspNetCore.Mvc;
using System;
using System.Web.Http.Routing;

namespace WebAPI.Controllers
{
    internal class PageLinkBuilder
    {
        private IUrlHelper url;
        private string route;
        private object routeValues;
        private object page;
        private object pagesize;
        private object total;

        public Uri FirstPage { get; private set; }
        public Uri LastPage { get; private set; }
        public Uri NextPage { get; private set; }
        public Uri PreviousPage { get; private set; }

        public PageLinkBuilder(IUrlHelper url, string route, object extraValues, int pageNum, int pageSize, long total)
        {
            Console.WriteLine(total);
            // Determine total number of pages
            var pageCount = total > 0
                ? (int)Math.Ceiling(total / (double)pageSize)
                : 0;

            this.url = url;
            this.route = route;
            this.routeValues = extraValues;
            this.page = pageNum;
            this.pagesize = pageSize;
            this.total = total;
            // Create them page links 
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