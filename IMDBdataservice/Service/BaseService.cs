using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBdataservice.Service
{
    class BaseService : IbaseService
    {
        public CharacterName GetCharacterName(int id, int Id)
        {
            throw new NotImplementedException();
        }

        public Person GetPerson(int id)
        {
            throw new NotImplementedException();
        }

        public Title GetTitle(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Title> GetTitles()
        {
            throw new NotImplementedException();
        }
    }
}
