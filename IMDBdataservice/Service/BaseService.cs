﻿using System;
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
            orderList = ctx.Titles.Where(x => x.PrimaryTitle.ToLower().Contains(search.ToLower())).ToList();
                return orderList;
        }

        public bool BookmarkMovie(string titleId, string userId)
        {
            BookmarkTitle bt = new()
            {
                TitleId = titleId,
                UserId = userId
            };

            ctx.Add(bt);
            return ctx.SaveChanges() > 0;
        }

        public void CommentMovie(string titleId, string comment)
        {
            
        }

        public void GetSearchHistory()
        {


        }

        public bool BookmarkPerson(string personId, string userId) {
            BookmarkPerson bt = new()
            {
                PersonId = personId,
                UserId = userId
            };

            ctx.Add(bt);
            return ctx.SaveChanges() > 0;
        }
        public void RateMovie() { }
        public void RatePerson() { }
        public void SearchByGenre() { }
        public void GetTop10HighesRatedMovies() { }
        public void GetMostFrequentPerson() { }
        public void SeeRatingOfMovie() { }

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
