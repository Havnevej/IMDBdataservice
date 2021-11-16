using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class User
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
