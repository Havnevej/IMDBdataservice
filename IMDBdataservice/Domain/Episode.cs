using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Episode
    {
        public string ParentTitleId { get; set; }
        public string TitleId { get; set; }
        public string SeasonNr { get; set; }
        public string EpisodeNr { get; set; }
    }
}
