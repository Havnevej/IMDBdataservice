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

        Profession GetProfession(int id, String type);

        Person GetPerson(int id);

        TitleRating GetTitleRating(int id);

        TitleVersion GetTitleVersion(int id);

        Principal GetPrincipal(int id, int ordering);

        CharacterName GetCharacterName(int id, String Name );

        IList<Title> GetTitles();

    }

}
