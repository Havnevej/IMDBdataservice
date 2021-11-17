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
        public bool UpdateTitle(Title originalTitle, Title updateTitle);
        public List<Title> SearchTitles(Title title, QueryString queryString);
        public bool BookmarkTitle(BookmarkTitle bt);
        public object CommentTitle(Comment comment);
        public bool RateTitle(string userId, string titleId, string rating);
        public Task<List<Title>> SearchTitleByGenre(QueryString queryString);
        public Task<List<Title>> GetTopTitles(QueryString queryString);
        public Task<List<Title>> GetRatingForTitle(string id);
        public List<Comment> GetCommentsByTitleId(string titleId, QueryString queryString);
        //              PERSON
        public Person GetPerson(string personId);
        public bool AddPerson(Person title);
        public bool RemovePerson(Person personToBeRemoved);
        public bool UpdatePerson(Person originalPerson, Person updatePerson);
        public List<Person> SearchPersons(Person person, QueryString queryString);
        public List<Person> GetMostFrequentPerson(QueryString queryString);
        public void RatePerson();
        public bool BookmarkPerson(BookmarkPerson bp);
        //              MISC
        public List<SearchHistory> GetSearchHistory();
        public imdbContext GetImdbContext();

        public User GetUser(string id);
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
