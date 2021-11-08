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
        public List<SearchHistory> GetSearchHistory();
        public bool BookmarkPerson(string personId, string userId);
        public void RateMovie();
        public void RatePerson();
        public void SearchByGenre();
        public List<Title> GetTop10HighesRatedMovies();
        public void GetMostFrequentPerson();
        //public Task<List<Title>> SeeRatingOfMovie(string input);


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
