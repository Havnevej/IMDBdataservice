using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Crew
    {
        public string TitleId { get; set; }
        public string PersonId { get; set; }
        public string PrimaryProfession { get; set; }
        public string AdditionalProfession { get; set; }
        public bool? IsPrincipal { get; set; }
        public int Ordering { get; set; }
    }
}
