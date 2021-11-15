using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBdataservice.Service
{
    public interface IbaseService
    {
        //Functions go here
        public imdbContext GetImdbContext();
        public Title GetTitle(string titleId);
        public List<Title> SearchTitles(string search);
        public List<Person> SearchPersons(string search);
        public Person GetPerson(string personId);
        public bool BookmarkTitle(string titleId, string userId);
        public void CommentTitle(string titleId, string comment);
        public List<SearchHistory> GetSearchHistory();
        public bool BookmarkPerson(string personId, string userId);
        public bool RateTitle(string userId, string titleId, string rating);
        public void RatePerson();
        public Task<List<Title>> SearchTitleByGenre(QueryString queryString);
        public Task<List<Title>> GetTopTitles(QueryString queryString);
        public Task<List<Person>> GetMostFrequentPerson(string id);
        public Task<List<Title>> SeeRatingOfTitle(string id);


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
