using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IMDBdataservice.Service
{
    public class BaseService : IbaseService
    {
        public static readonly imdbContext ctx = new();
        public imdbContext GetImdbContext (){ return ctx; }


        /*
         * 
         * 
         * 
         *  TITLES
         * 
         * 
         */
        public List<Title> SearchTitles(Title title, QueryString queryString)
        {
            List<Title> orderList = new();
            orderList = ctx.Titles.Where(x => x.PrimaryTitle.Contains(title.PrimaryTitle)).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToList();
                return orderList;
        }
        public Title GetTitle(string id)
        {
            var title = ctx.Titles.FirstOrDefault(x => x.TitleId == id);
            return title;
        }
        public async Task<List<Title>> GetRatingForTitle(string id)
        {
            List<Title> returns = new();
            await ctx.Titles.Include(x => x.TitleRating).Where(x => x.TitleId == id).ForEachAsync(x =>
            {
                returns.Add(new Title
                {
                    PrimaryTitle = x.PrimaryTitle,
                    TitleRating = x.TitleRating
                });
            });

            return returns;
        }
        public bool BookmarkTitle(BookmarkTitle bt)
        {
            if (!ctx.BookmarkTitles.ToList().Any(x => x.UserId == bt.UserId && x.TitleId == bt.TitleId))
            {
                 ctx.Add(bt);
                 return ctx.SaveChanges() > 0;
            }
            return false;
        }

        public object CommentTitle(Comment comment) //good to have but not prio.
        {

            ctx.Add(comment);

            ctx.SaveChanges();
            return new{Error="false",ErrorMessage=""};
        }

        public List<Comment> GetCommentsByTitleId(string TitleId, QueryString queryString)
        {
            List<Comment> Result = ctx.Comments.Where(x => x.TitleId == TitleId).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToList();
            return Result;
        }

        public bool RateTitle(string userId, string titleId, string rating)
        {
            UserTitleRating rt = new()
            {
                TitleId = titleId,
                UserId = userId,
                Rating = rating
            };

            ctx.Add(rt);
            return ctx.SaveChanges() > 0;

        }
        public async Task<List<Title>> SearchTitleByGenre(QueryString queryString)
        {
            List<Title> result = new();
#warning This has changed, genres is a list, make new logic
            result = ctx.Titles.Include(x => x.Genres).Include(x => x.TitleRating).Where(x => x.Genres.Any(x => x.GenreName == queryString.Genre)).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToListAsync().Result;
            return result;
        }

        public async Task<List<Title>> GetTopTitles(QueryString queryString)
        { //works but too many top movies with rating 10*
            List<Title> result = new();
            result = ctx.Titles.Include(x => x.TitleRating).OrderByDescending(x => x.TitleRating.RatingAvg).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToListAsync().Result; 
            return result;
        }

        /*
         * 
         * 
         * 
         *  User
         * 
         * 
         */
        public List<SearchHistory> GetSearchHistory()
        {
            List<SearchHistory> result = new();
            result = ctx.SearchHistories.ToList();
            return result;
        }

        /*
         * 
         * 
         * 
         *  Person
         * 
         * 
         */
        public bool BookmarkPerson(BookmarkPerson bp)
        {
            if (!ctx.BookmarkPeople.ToList().Any(x => x.UserId == bp.UserId && x.PersonId == bp.PersonId))
            {
                ctx.Add(bp);
                return ctx.SaveChanges() > 0;
            }
            return false;
        }

        public void RatePerson() {}

        public List<Person> GetMostFrequentPerson(QueryString queryString) { //freq actor based on another actor and their work together. [GetMostFrequentCoWorker]
            List<Person> result = new();
            //Find all co-workers:
            //result = ctx.People.Include(x => x.KnownForTitles).Where(x => x.KnownForTitles.Any(x=>x.PersonId == queryString.personId)).ToList();

            //Find all titles with our rockstar in it:
            List<Title> result_t = new();
            result_t = ctx.Titles.Include(x => x.KnownForTitles).Where(x => x.KnownForTitles.Any(x => x.PersonId == queryString.personId)).ToList();

            //Find the most frequent co-star:
            result_t.ForEach(x => {
                List<Person> tt = new();
                tt = ctx.People.Include(p => p.KnownForTitles).Where(p => p.KnownForTitles.Any(f => f.TitleId == x.TitleId)).ToList();
                tt.ForEach(x => result.Add(x));
            });
            //find co-stars that have been in more than one movie with our rock star:
            var toremove = result.FirstOrDefault(x => x.PersonId == queryString.personId);
            result.Remove(toremove);
            
            var ss = result.GroupBy(x => x)
              .OrderByDescending(g => g.Count()).Take(5)
              .Select(y => y.Key)
              .ToList();
            return ss;
        }

        public List<Person> SearchPersons(Person person, QueryString queryString) // Done in controller
        {
            List<Person> orderList = new();
            orderList = ctx.People.Where(x => x.PersonName.Contains(person.PersonName)).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToList();
            return orderList;
        }
        public Person GetPerson(string id) //Done in controller
        {
            var person = ctx.People.FirstOrDefault(x => x.PersonId == id);
            return person;
        }
        // Not implemented functions
        public bool AddTitle(Title title)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTitle(Title originalTitle, Title updateTitle)
        {
            throw new NotImplementedException();
        }

        public bool AddPerson(Person title)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePerson(Person originalPerson, Person updatePerson)
        {
            throw new NotImplementedException();
        }

        public bool RemoveTitle(Title titleToBeRemoved)
        {
            throw new NotImplementedException();
        }

        public bool RemovePerson(Person personToBeRemoved)
        {
            throw new NotImplementedException();
        }


        public User GetUser(string id)
        {
            User person = ctx.Users.FirstOrDefault(x => x.UserId == id);
            return person;
        }

        public void CreateUser(string userid, string username, string password = null, string salt = null)
        {
            User user = new()
            {
                UserId = userid,
                UserName = username,
                Password = password,
                Salt = salt
            };
            ctx.Add(user);
            ctx.SaveChanges();
        }
        public void DeleteUser(string username)
        {
            var user = ctx.Users.FirstOrDefault(x => x.UserName == username);
            ctx.Users.Remove(user);
            ctx.SaveChanges();
        }


        #region functions todo

        /*
        public Profession GetProfession(int id, string type)
        {
            throw new NotImplementedException();
        }

        public Person GetPerson(int id)
        {
            throw new NotImplementedException();
        }

        public TitleRating GetTitleRating(int id)
        {
            throw new NotImplementedException();
        }

        public TitleVersion GetTitleVersion(int id)
        {
            throw new NotImplementedException();
        }

        public Principal GetPrincipal(int id, int ordering)
        {
            throw new NotImplementedException();
        }

        public CharacterName GetCharacterName(int id, string Name){throw new NotImplementedException();}
        */
        #endregion
    }
}
