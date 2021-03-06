using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace IMDBdataservice.Service
{
    public class BaseService : IbaseService
    {
        public imdbContext ctx = new();
        public imdbContext GetImdbContext (){ return ctx; }
        public static BaseService GetBaseService() { return new BaseService(); }
        public BaseService()
        {
            ctx = new imdbContext();
        }

        /*
         * 
         * 
         * 
         *  TITLES
         * 
         * 
         */
        public List<Title> SearchTitles(QueryStringOur queryString, User u)
        {
            List<Title> titleList = new();

            if (u != null)
            {
                User user = ctx.Users.FirstOrDefault(x => x.Username.ToLower() == u.Username.ToLower());
                SearchHistory search = new()
                {
                    Username = user.Username,
                    SearchString = queryString.needle,
                    SearchDate = DateTime.Now
                };
                ctx.Add(search);
                ctx.SaveChanges();
            }

            titleList = ctx.Titles.
                Include(x => x.omdb).
                Include(x => x.director.person).
                Include(g => g.Genres).
                Include(r => r.TitleRating).
                Where(x => x.PrimaryTitle.ToLower().Contains(queryString.needle.ToLower()))
                .OrderByDescending(x => Convert.ToInt32(x.TitleRating.Votes))
                .Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize)
                .ToList(); 
            titleList.ForEach(x =>
            {
                x.principal = ctx.Principals.Include(p => p.person).
                Where(z => z.TitleId == x.TitleId).ToListAsync().Result;
            });

            return titleList;
        }
        public Title GetTitle(string id)
        {
            var title = ctx.Titles.
                Include(g => g.Genres).
                Include(x => x.omdb).
                Include(xx => xx.director.person).
                Include(d=>d.TitleRating).
                FirstOrDefault(x => x.TitleId == id);
            if (title != null)
            {
                title.principal = ctx.Principals.Include(x => x.person).
                Where(x => x.TitleId == id && (x.Category == "actor" || x.Category == "actress")).ToListAsync().Result;

            }
            return  title;
        }

        
        public float GetRatingForTitle(string id)
        {
            var avg_rating = ctx.Titles.Include(x => x.TitleRating).Where(x => x.TitleId == id).FirstOrDefault().TitleRating.RatingAvg;
            return (float)avg_rating;
        }
        public List<BookmarkTitle> GetBookmarksForUser(string username) //get stars
        {
            List<BookmarkTitle> returns = ctx.BookmarkTitles.Where(x => x.Username == username).ToListAsync().Result;
            return returns;
        }
        public bool BookmarkTitle(BookmarkTitle bt)
        {
            if (!ctx.BookmarkTitles.ToList().Any(x => x.Username == bt.Username && x.TitleId == bt.TitleId))
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

        public List<Comment> GetCommentsByTitleId(string TitleId, QueryStringOur queryString)
        {
            List<Comment> Result = ctx.Comments.Where(x => x.TitleId == TitleId).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToListAsync().Result;
            return Result;
        }

        public bool RateTitle(UserTitleRating urt)
        {
            if (!ctx.UserTitleRatings.Any(x => x.Username == urt.Username && x.TitleId == urt.TitleId && x.Rating == urt.Rating)){
                ctx.Add(urt);
                return ctx.SaveChanges() > 0;

            }
            return false;

        }

        public async Task<List<Title>> SearchTitleByGenre(QueryStringOur queryString, User u = null)
        {
            List<Title> result = new();

            
            if (u != null)
            {
                User user = ctx.Users.FirstOrDefault(x => x.Username.ToLower() == u.Username.ToLower());
                SearchHistory search = new()
                {
                    Username = user.Username,
                    SearchString = queryString.Genre,
                    SearchDate = DateTime.Now
                };
                ctx.Add(search);
                ctx.SaveChanges();
            }

            result = ctx.Titles.Include(x => x.Genres).Include(x => x.TitleRating).Where(x => x.Genres.Any(x => x.GenreName == queryString.Genre)).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToListAsync().Result;
            return result;
        }

        public async Task<List<Title>> GetTopTitles(QueryStringOur queryString)
        { //works but too many top movies with rating 10*
            List<Title> result = new();
            if(queryString.Genre != null)
            {
                result = ctx.Titles.Include(x => x.Genres).Where(x => x.Genres.Any(genre=>genre.GenreName == queryString.Genre)).Include(x => x.TitleRating).Include(x => x.omdb)
                 .Where(x => x.TitleType != "tvEpisode")
                 .OrderByDescending(x => Convert.ToInt32(x.TitleRating.Votes))
                 .Skip(queryString.Page * queryString.PageSize)
                 .Take(queryString.PageSize).ToListAsync().Result;
            } else
            {
                result = ctx.Titles.Include(x => x.Genres).Include(x => x.TitleRating).Include(x => x.omdb).
                     Where(x => x.TitleType != "tvEpisode").
                     OrderByDescending(x => Convert.ToInt32(x.TitleRating.Votes)).Skip(queryString.Page * queryString.PageSize)
                     .Take(queryString.PageSize).ToListAsync().Result;
            }
 
            return result;
        }
        public List<string> GetGenres()
        {
            return ctx.Genres.Select(genre => genre.GenreName).Distinct().ToListAsync().Result.OrderBy(t => t).ToList();
        }
        public String AddTitle(Title title)
        {
            if (!ctx.Titles.ToList().Any(x => x.TitleId == title.TitleId))
            {
                int nextId = ctx.Titles.ToListAsync().Result.Max(title => Convert.ToInt32(int.Parse(title.TitleId.Split("tt")[1])))+1;
                title.TitleId = "tt" + nextId.ToString();
                ctx.Add(title);
                ctx.SaveChanges();
                return title.TitleId;
            }
            return "error";
        }
        public bool UpdateTitle(TitleDTO title)
        {
            var dbTitle = GetTitle(title.TitleId);
            if (dbTitle == null)
            {
                Console.WriteLine("hey");
                return false;
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TitleDTO, Title>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            });

            var mapper = new Mapper(config);
            dbTitle = mapper.Map<TitleDTO, Title>(title, dbTitle);

            return ctx.SaveChanges() > 0;
        }

        public bool RemoveTitle(Title titleToBeRemoved)
        {
            if (ctx.Titles.ToList().Any(x => x.TitleId == titleToBeRemoved.TitleId))
            {

                var titleDelete = ctx.Titles.FirstOrDefault(x => x.TitleId == titleToBeRemoved.TitleId);
                ctx.Titles.Remove(titleDelete);

                return ctx.SaveChanges() > 0;
            }
            return false;
        }

        /*
         * 
         * 
         * 
         *  User
         * 
         * 
         */

        public User GetUser(string username)
        {
            User user = ctx.Users.FirstOrDefault(x => x.Username == username);
            if(user != null)
            {
                user.UserTitleRating = ctx.UserTitleRatings.Where(rating => rating.Username == username).Include(title => title.Title).ToListAsync().Result;
            }
            return user;
        }
        public List<Comment> GetCommentsByUser(string username, QueryStringOur queryString) // Done in controller
        {
            List<Comment> commentList = new();
            commentList = ctx.Comments.Where(x => x.Username == username).Skip(0)
                .Take(queryString.PageSize).ToList();
            return commentList;
        }
        public void CreateUser(string username, string password = null, string salt = null)
        {
            long nextId;
            if (ctx.Users.Count() == 0)
                nextId = 1;
            else nextId = ctx.Users.Max(x => x.UserId) + 1;

            User user = new()
            {
                UserId = nextId,
                Username = username,
                Password = password,
                Salt = salt,
                CreatedDate = DateTime.Now
            };
            ctx.Add(user);
            ctx.SaveChanges();
        }


        public void DeleteUser(string username)
        {
            var user = ctx.Users.FirstOrDefault(x => x.Username == username);
            ctx.Users.Remove(user);
            ctx.SaveChanges();
        }

        public List<SearchHistory> GetSearchHistory(string username, QueryStringOur queryString)
        {
            List<SearchHistory> result = new();
            result = ctx.SearchHistories.Where(x => x.Username == username).Take(10).ToList();
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
            if (!ctx.BookmarkPeople.ToList().Any(x => x.Username == bp.Username && x.PersonId == bp.PersonId))
            {
                ctx.Add(bp);
                return ctx.SaveChanges() > 0;
            }
            return false;
        }

        public List<Person> GetMostFrequentPerson(QueryStringOur queryString) { //freq actor based on another actor and their work together. [GetMostFrequentCoWorker]
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

        public List<Person> SearchPersons(QueryStringOur queryString) // Done in controller
        {
            List<Person> orderList = new();
            orderList = ctx.People.Where(x => x.PersonName.Contains(queryString.needle)).Skip(queryString.Page * queryString.PageSize)
                .Take(queryString.PageSize).ToList();
            return orderList;
        }
        public Person GetPerson(string id) //Done in controller
        {
            var person = ctx.People.FirstOrDefault(x => x.PersonId == id);
            return person;
        }

        public bool AddPerson(Person person)
        {
            if (!ctx.People.ToList().Any(x => x.PersonId == person.PersonId)) {
                    
                ctx.Add(person);
                return ctx.SaveChanges() > 0;
            }
            return false;
        
        }

        public bool UpdatePerson(PersonDTO person)
        {
            var dbPerson = GetPerson(person.PersonId);
            if (dbPerson == null)
            {
  
                return false;
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<PersonDTO, Person>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            });

            var mapper = new Mapper(config);
            dbPerson = mapper.Map<PersonDTO, Person>(person, dbPerson);

            return ctx.SaveChanges() > 0;
        }

        public bool RemovePerson(string personId)
        {
            if (ctx.People.ToList().Any(x => x.PersonId == personId))
            {
                var personRemove = ctx.People.FirstOrDefault(x => x.PersonId ==personId);
                ctx.People.Remove(personRemove);

                return ctx.SaveChanges() > 0;
            }
            return false;
        }
    }
}
