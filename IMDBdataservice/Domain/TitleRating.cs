using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class TitleRating
    {
        public string TitleId { get; set; }
        public float? RatingAvg { get; set; }
        public string Votes { get; set; }

        public virtual Title Title { get; set; }
    }
}
