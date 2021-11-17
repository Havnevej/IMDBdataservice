using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Title
    {
        public Title()
        {
            BookmarkTitles = new HashSet<BookmarkTitle>();
            CharacterNames = new HashSet<CharacterName>();
            Comments = new HashSet<Comment>();
            Genres = new HashSet<Genre>();
            KnownForTitles = new HashSet<KnownForTitle>();
            UserTitleRatings = new HashSet<UserTitleRating>();
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
        public virtual ICollection<BookmarkTitle> BookmarkTitles { get; set; }
        public virtual ICollection<CharacterName> CharacterNames { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<KnownForTitle> KnownForTitles { get; set; }
        public virtual ICollection<UserTitleRating> UserTitleRatings { get; set; }
    }
}
