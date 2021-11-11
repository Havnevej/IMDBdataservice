using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IMDBdataservice.Service
{
    class BaseService : IbaseService
    {
        private static readonly imdbContext ctx = new();


        /*
         * 
         * 
         * 
         *  TITLES
         * 
         * 
         */
        public List<Title> SearchTitles(string search)
        {
            List<Title> orderList = new();
            orderList = ctx.Titles.Where(x => x.PrimaryTitle.ToLower().Contains(search.ToLower())).ToList();
                return orderList;
        }

        public Title GetTitles(string id)
        {
            Title title = new();
            title = ctx.Titles.FirstOrDefault(x => x.TitleId == id);
            return title;
        }

        public async Task<List<Title>> SeeRatingOfTitle(string id)
        {
            List<Title> returns = new();
            await ctx.Titles.Include(x => x.titlerating).Where(x => x.TitleId == id).ForEachAsync(x =>
            {
                returns.Add(new Title
                {
                    PrimaryTitle = x.PrimaryTitle,
                    titlerating = x.titlerating
                });
            });

            return returns;
        }
        public bool BookmarkTitle(string titleId, string userId)
        {
            BookmarkTitle bt = new()
            {
                TitleId = titleId,
                UserId = userId
            };

            ctx.Add(bt);
            return ctx.SaveChanges() > 0;
        }

        public void CommentTitle(string titleId, string comment) //good to have but not prio.
        {
            
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
        public async Task<List<Title>> SearchTitleByGenre(string genre)
        {
            List<Title> result = new();
            result = ctx.Titles.Include(x => x.genre).Where(x => x.genre.ToString() == genre).ToList();
            return result;
        }
        public async Task<List<Title>> GetTopTitles(int top)
        { //works but too many top movies with rating 10
            List<Title> result = new();
            result = ctx.Titles.Include(x => x.titlerating).OrderByDescending(x => x.titlerating).Take(top).ToList();
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
            List<Person> result = new();
            List<Title> titles_list = ctx.Titles.Include(x=>x.knownForTitles).Where(x => x.knownForTitles == id).ToList();
            
            titles_list.ForEach(x => {
                List<Person> tt = new();
                tt = ctx.People.Include(p => p.knownForTitles).Where(p => p.knownForTitles == x.TitleId).ToList();
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

        public Person GetPerson(string id)//
        {
            Person person = new();
            person = ctx.People.FirstOrDefault(x=>x.PersonId==id);
            return person;
        }


        Title IbaseService.GetTitle(string titleId)
        {
            throw new NotImplementedException();
        }

        Person IbaseService.GetPerson(string personId)
        {
            throw new NotImplementedException();
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
