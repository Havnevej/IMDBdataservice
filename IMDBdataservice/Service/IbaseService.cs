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
        public List<Person> GetPerson(string search);
        public bool BookmarkMovie(string titleId, string userId);
        public void CommentMovie(string titleId, string comment);
        public List<SearchHistory> GetSearchHistory();
        public bool BookmarkPerson(string personId, string userId);
        public bool RateMovie(string userId, string titleId, string rating);
        public void RatePerson();
        public Task<List<Title>> SearchByGenre(string genre);
        public Task<List<Title>> GetTop10HighesRatedMovies();
        public Task<List<Person>> GetMostFrequentPerson(string id);
        public Task<List<Title>> SeeRatingOfMovie(string id);


        /*
        Title GetTitle(int id);
        Profession GetProfession(int id, String type);
        Person GetPerson(string search);
        TitleRating GetTitleRating(int id);
        TitleVersion GetTitleVersion(int id);
        Principal GetPrincipal(int id, int ordering);
        CharacterName GetCharacterName(int id, String Name);
        IList<Title> GetTitles();
        */

    }
}
