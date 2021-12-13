using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Director
    {
        public string TitleId { get; set; }
        public string PersonId { get; set; }
        
        public Person person { get; set; }
    }
}
