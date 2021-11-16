using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBdataservice
{
    public partial class PersonDTO
    {
        public PersonDTO()
        {
            KnownForTitles = new HashSet<KnownForTitle>();
        }

        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string Birthyear { get; set; }
        public string Deathyear { get; set; }

        public virtual ICollection<KnownForTitle> KnownForTitles { get; set; }
    }
}
