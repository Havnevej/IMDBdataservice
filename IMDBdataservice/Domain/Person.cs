using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Person
    {
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string Birthyear { get; set; }
        public string Deathyear { get; set; }
        public KnownForTitle knownForTitles { get; set; }
    }
}
