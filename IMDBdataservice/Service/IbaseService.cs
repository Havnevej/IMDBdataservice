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
        public List<Title> GetTitle(string search);
        public bool BookmarkMovie(string titleId, string userId);
        public void CommentMovie(string titleId, string comment);
        public void GetSearchHistory();
        public bool BookmarkPerson(string personId, string userId);
        public void RateMovie();
        public void RatePerson();
        public void SearchByGenre();
        public void GetTop10HighesRatedMovies();
        public void GetMostFrequentPerson();
        public void SeeRatingOfMovie();


        /*
        Title GetTitle(int id);
        Profession GetProfession(int id, String type);
        Person GetPerson(int id);
        TitleRating GetTitleRating(int id);
        TitleVersion GetTitleVersion(int id);
        Principal GetPrincipal(int id, int ordering);
        CharacterName GetCharacterName(int id, String Name );
        IList<Title> GetTitles();
        */

    }
}
