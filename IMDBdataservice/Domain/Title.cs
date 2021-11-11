using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Title
    {
        public Title()
        {
            Genres = new HashSet<Genre>();
            KnownForTitles = new HashSet<KnownForTitle>();
        }

        public string TitleId { get; set; }
        public string TitleType { get; set; }
        public string OriginalTitle { get; set; }
        public string PrimaryTitle { get; set; }
        public bool? IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }

        public virtual TitleRating TitleRating { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<KnownForTitle> KnownForTitles { get; set; }
    }
}
