using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class TitleVersion
    {
        public string TitleId { get; set; }
        public string TitleVersion1 { get; set; }
        public string TitleName { get; set; }
        public string Region { get; set; }
        public string Language { get; set; }
        public string Types { get; set; }
        public string Attributes { get; set; }
        public bool? IsOriginalTitle { get; set; }
    }
}
