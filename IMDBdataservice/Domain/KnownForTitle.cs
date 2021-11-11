using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class KnownForTitle
    {
        public string PersonId { get; set; }
        public string TitleId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Title Title { get; set; }
    }
}
