using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class Person
    {
        public Person()
        {
            CharacterNames = new HashSet<CharacterName>();
            KnownForTitles = new HashSet<KnownForTitle>();
        }

        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string Birthyear { get; set; }
        public string Deathyear { get; set; }

        public virtual ICollection<CharacterName> CharacterNames { get; set; }
        public virtual ICollection<KnownForTitle> KnownForTitles { get; set; }
    }
}
