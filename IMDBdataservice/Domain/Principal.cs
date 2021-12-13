using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Principal
    {
        public string TitleId { get; set; }
        public int Ordering { get; set; }
        public string PersonId { get; set; }
        public string Category { get; set; }
        public string Job { get; set; }

        public Person person { get; set; }
    }
}
