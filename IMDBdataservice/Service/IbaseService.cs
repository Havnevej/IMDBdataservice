using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBdataservice.Service
{
    interface IbaseService
    {
        //Functions go here
        Title GetTitle(int id);
        Person GetPerson(int id);
        CharacterName GetCharacterName(int id, int Id); // Det må ikke hedde det samme::D
        IList<Title> GetTitles();
    }

}
