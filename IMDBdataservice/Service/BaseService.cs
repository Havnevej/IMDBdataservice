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

        public List<Title> GetTitle(string search)
        {
            List<Title> orderList = new();
            orderList = ctx.Titles.Where(x => x.PrimaryTitle == search).ToListAsync().Result;

            return orderList;
        }


        public void BookmarkMovie(string titleId, string userId)
        {
            BookmarkTitle bt = new()
            {
                TitleId = titleId,
                UserId = userId
            };

            //shouldnt this be autoincrementet in the db?
            //bt.TitleId = ctx.BookmarkTitles.Max(x => x.TitleId) + 1;
            ctx.Add(bt);
            ctx.SaveChanges();

        }

        public void CommentMovie(string titleId, string comment)
        {
           //dont have a comment table, should just add in the project + mapping?
        }

        public void GetSearchHistory()
        {


        }

        public void BookmarkPerson() { }
        public void RateMovie() { }
        public void RatePerson() { }
        public void SearchByGenre() { }
        public void GetTop10HighesRatedMovies() { }
        public void GetMostFrequentPerson() { }
        public void SeeRatingOfMovie() { }

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

        public CharacterName GetCharacterName(int id, string Name)
        {
            throw new NotImplementedException();
        }
        */
        
    }
}
