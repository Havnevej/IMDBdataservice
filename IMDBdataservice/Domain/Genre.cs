using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Genre
    {
        public string TitleId { get; set; }
        public string GenreName { get; set; }

        public virtual Title Title { get; set; }
    }
}
