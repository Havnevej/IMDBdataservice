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
        public async Task<List<Title>> SeeRatingOfTitle(string id)
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
        public bool BookmarkPerson(string personId, string userId) {
            BookmarkPerson bt = new()
            {
                PersonId = personId,
                UserId = userId
            };

            ctx.Add(bt);
            return ctx.SaveChanges() > 0;
        }

        public void RatePerson() {}

        public async Task<List<Person>> GetMostFrequentPerson(string id) { //freq actor based on another actor and their work together. [GetMostFrequentCoWorker]
            List<Person> result = new(); // Changed function
#warning Changed function with new context, check if works
            List<Title> titles_list = ctx.Titles.Include(x=>x.KnownForTitles).Where(x => x.KnownForTitles.FirstOrDefault().ToString().Contains(id)).ToList();
            
            titles_list.ForEach(x => {
                List<Person> tt = new();
                tt = ctx.People.Include(p => p).Where(p => p.PersonId == x.TitleId).ToList();
                tt.ForEach(x => result.Add(x));
            });

            return result;
        }

        public List<Person> SearchPersons(string search)
        {
            List<Person> orderList = new();
            orderList = ctx.People.Where(x => x.PersonName.ToLower().Contains(search.ToLower())).ToList();
            return orderList;
        }
        public Person GetPerson(string id)
        {
            var person = ctx.People.FirstOrDefault(x => x.PersonId == id);
            return person;
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
