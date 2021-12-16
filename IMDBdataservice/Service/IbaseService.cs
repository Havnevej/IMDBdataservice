using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBdataservice.Service
{
    public interface IbaseService
    {
        //Functions go here
        //              TITLE
        public Title GetTitle(string titleId);
        public bool AddTitle(Title title);
        public bool RemoveTitle(Title titleToBeRemoved);
        public bool UpdateTitle(TitleDTO title);
        public List<Title> SearchTitles(QueryStringOur queryString);
        public bool BookmarkTitle(BookmarkTitle bt);
        public object CommentTitle(Comment comment);
        public bool RateTitle(UserTitleRating urt);
        public Task<List<Title>> SearchTitleByGenre(QueryStringOur queryString);
        public Task<List<Title>> GetTopTitles(QueryStringOur queryString);
        public float GetRatingForTitle(string id);
        public List<Comment> GetCommentsByTitleId(string titleId, QueryStringOur queryString);
        //              PERSON
        public Person GetPerson(string personId);
        public bool AddPerson(Person title);
        public bool RemovePerson(string personId);
        public bool UpdatePerson(PersonDTO person);
        public List<Person> SearchPersons(QueryStringOur queryString);
        public List<Person> GetMostFrequentPerson(QueryStringOur queryString);
        public bool BookmarkPerson(BookmarkPerson bp);
        //              MISC
        public List<SearchHistory> GetSearchHistory(string username, QueryStringOur queryString);
        public imdbContext GetImdbContext();
        //              USER
        public User GetUser(string username);
        public void CreateUser(string username, string password = null, string salt = null);
        public void DeleteUser(string username);


        /*
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
