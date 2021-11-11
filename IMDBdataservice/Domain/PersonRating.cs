using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class PersonRating
    {
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public double? Weight { get; set; }
        public long? Rating { get; set; }
        public long? NumVotes { get; set; }
    }
}
