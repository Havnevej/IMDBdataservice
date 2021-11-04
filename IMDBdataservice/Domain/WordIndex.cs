using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class WordIndex
    {
        public string TitleId { get; set; }
        public string Word { get; set; }
        public string Field { get; set; }
        public string Lexeme { get; set; }
    }
}
