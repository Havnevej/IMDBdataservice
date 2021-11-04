using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class UserTitleRating
    {
        public string UserId { get; set; }
        public string TitleId { get; set; }
        public string Rating { get; set; }
    }
}
