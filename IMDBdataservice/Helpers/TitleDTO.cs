using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public class TitleDTO
    {
        public string href { get; set; }
        public string TitleId { get; set; }
        public string TitleType { get; set; }
        public string OriginalTitle { get; set; }
        public string PrimaryTitle { get; set; }
        public bool? IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }

    }
}
