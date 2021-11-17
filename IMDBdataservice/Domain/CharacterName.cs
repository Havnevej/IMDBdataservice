using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class CharacterName
    {
        public string CharacterName1 { get; set; }
        public string PersonId { get; set; }
        public string TitleId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Title Title { get; set; }
    }
}
