using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class SearchHistory
    {
        public string Username { get; set; }
        public string SearchString { get; set; }
        public string Date { get; set; }
    }
}
