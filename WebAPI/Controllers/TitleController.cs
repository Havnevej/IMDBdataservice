using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using IMDBdataservice.Service;
using IMDBdataservice;
using Microsoft.AspNetCore.Routing;
using WebServiceToken.Attributes;
using Microsoft.AspNetCore.Authorization;
using WebServiceToken.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/titles")]
    public class TitleController : ControllerBase, IOurcontroller<Title>
    {
        BaseService _dataService;
        LinkGenerator _linkGenerator;
        static readonly MapperConfiguration configSingle = new MapperConfiguration(cfg => {
            cfg.CreateMap<Title, TitleDTO> ().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        });
        MapperConfiguration configMulti = new MapperConfiguration(cfg => cfg.CreateMap<Title, TitleDTO>());
        static Mapper mapper;
        
        private List<TitleDTO> ConvertToTitleDto(List<Title> input)
        {
            mapper = new(configMulti);
            List<TitleDTO> titles = mapper.Map<List<TitleDTO>>(input);
            foreach (var item in titles)
            {
                item.href = Url.RouteUrl("").ToString()+$"/{item.TitleId}";
            }
            return titles;
        }
        public TitleController(ILogger<TitleController> logger,  LinkGenerator linkGenerator)
        {
            _dataService = new BaseService();
            _linkGenerator = linkGenerator;
        }


        [HttpGet("{id}", Name = nameof(GetTitle))]
        public IActionResult GetTitle(string id)
        {
            var title = _dataService.GetTitle(id);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }

        

        [HttpGet(Name = nameof(GetTitles))]
        public IActionResult GetTitles([FromQuery] QueryStringOur queryString)
        {
            var title = _dataService.GetTopTitles(queryString).Result;

            //var numberOfProducts = _dataService.NumberOfTopTitles();

            long total = _dataService.GetImdbContext().Titles.Count();

            var pages = (int)Math.Ceiling((double)total / queryString.PageSize);


            var prev = (string)null;
            if (queryString.Page > 0)
            {
                prev = Url.Link(nameof(GetTitles), new QueryStringOur { Page = queryString.Page - 1, PageSize = queryString.PageSize});
            }
            var next = (string)null;
            if (queryString.Page < pages - 1)
            {
                next = Url.Link(nameof(GetTitles), new QueryStringOur { Page = queryString.Page +1, PageSize = queryString.PageSize });
            }

            if (title == null)
            {
                return NotFound();
            } else
            {
                var result = new
                {
                    pageSizes = new int[] { 10, 15, 20 },
                    count = total,
                    pages,
                    prev,
                    next,
                    title
                };
                return Ok(result);
            }
            
        
            //long total = _dataService.GetImdbContext().Titles.Count();
            //var linkBuilder = new PageLinkBuilder(Url, "", null, queryString.Page, queryString.PageSize, total);
            
            //return Ok(new {Data=ConvertToTitleDto(title), Paging = linkBuilder });
        }
        
        [HttpGet]
        [Route("search")]
        public IActionResult SearchTitleByGenre([FromQuery] QueryStringOur queryString)
        {
            List<Title> result = new();
            long total = 0;
            if (queryString.Genre == null)
            {
                result = _dataService.SearchTitles(queryString);
                total = _dataService.GetImdbContext().Titles.
                    Include(x => x.Genres).
                    Include(x => x.TitleRating).
                    Where(x => x.PrimaryTitle == queryString.PrimaryTitle).ToList().Count;
                    //Where(x => x.Genres.Any(x => x.GenreName == queryString.Genre)).ToList().Count;
            } else {
                result = _dataService.SearchTitleByGenre(queryString).Result;
                total = _dataService.GetImdbContext().Titles.Include(x => x.Genres).Include(x => x.TitleRating).
                    Where(x => x.Genres.Any(x => x.GenreName == queryString.Genre)).ToList().Count;
            }
            
            if (result.Count == 0)
            {
                return NotFound();
            }
            var linkBuilder = new PageLinkBuilder(Url, "", new{genre=queryString.Genre,needle=queryString.needle}, queryString.Page, queryString.PageSize, total);
            return Ok(new { Data = ConvertToTitleDto(result), Paging = linkBuilder });
        }

        [Authorization]
        [HttpGet]
        [Route("gethistory")]
        public IActionResult GetSearchHistory([FromQuery] QueryStringOur queryString)
        {
            // var username = HttpContext.Items["User"].ToString();
            User user = (User)HttpContext.Items["User"];
            Console.WriteLine("Username: " + user.Username);
            var history = _dataService.GetSearchHistory(user.Username, queryString);
       
                if (history == null)
                {
                    return NotFound();
                }

                return Ok(history); // only 10, no paging
        }

        [Authorization]
        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveTitle([FromBody] Title t)
        {
            User user = (User)HttpContext.Items["User"];
            Console.WriteLine(user.IsAdmin);
            if (user.IsAdmin == true)
            {
                if (_dataService.RemoveTitle(t))
                {
                    return Ok(new { REMOVED = t });
                }
                else
                {
                    return BadRequest(new { ERROR = "Could not remove", ERROR_TYPE = "UNKNOWN", DESC = t });
                }
            } else
            {
                return StatusCode(401, new { ERROR = "Only admins can delete titles", ERROR_TYPE = "NOT_ALLOWED" });
            }
        }
                    
        [Authorization]
        [HttpPost] // TODO: Add More error handling
        [Route("bookmark/title")]
        public IActionResult BookmarkTitle([FromBody] BookmarkTitle bt) 
        {
            if (_dataService.BookmarkTitle(bt))
            {
               return Ok("inserted");
            }
            else
            {
                return BadRequest("Already exists");
            }
        }
        [Authorization]
        [HttpPost] // TODO: Add More error handling
        [Route("comments")] 
        public IActionResult CommentTitle([FromBody] Comment comment)
        {
            if(comment.Comment1.Length >= 1024) { return BadRequest(new { ERROR = "Too long", ERROR_TYPE = "BAD_DATA" }); }; //deny comments that are long
            if(comment.TitleId == null || !_dataService.ctx.Titles.Any(t => t.TitleId == comment.TitleId)) { return BadRequest(new { ERROR = "Invalid title id", ERROR_TYPE = "BAD_DATA" }); };
            User user = (User)HttpContext.Items["User"];
            comment.Username = user.Username;
            comment.Date = DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss");

            _dataService.CommentTitle(comment);
            return CreatedAtAction(nameof(GetCommentsByTitleId), new { id = comment.TitleId.ToString() },comment);
        }

        [HttpGet]
        [Route("comments/{id}")] 
        public IActionResult GetCommentsByTitleId(string id, [FromQuery] QueryStringOur queryString)
        {
            var result = _dataService.GetCommentsByTitleId(id, queryString);
            if (result.Count == 0)
            {
                return BadRequest(new {ERROR="No comments for that id", ERROR_TYPE="BAD_DATA"});
            }
            return Ok(result);
        }
        [Authorization]
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateTitle(string id, [FromBody] TitleDTO title)
        {
            User user = (User)HttpContext.Items["User"];
            Console.WriteLine(user.IsAdmin);
            if (user.IsAdmin == null || user.IsAdmin == false)
            {
                return StatusCode(401, new { ERROR="Only admins can update titles", ERROR_TYPE="NOT_ALLOWED"});
            }
            title.TitleId = id;
            if (!_dataService.GetImdbContext().Titles.Any(x => x.TitleId == id))
            {
                return BadRequest(new { ERROR="title does not exist", ERROR_TYPE="NO_DATA"});
            }
            bool updated = _dataService.UpdateTitle(title);
            return CreatedAtAction(nameof(GetTitle),new {id=title.TitleId},new {UPDATED_TITLE = updated});

        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTitle([FromBody] Title title)
        {
            var result = _dataService.AddTitle(title);
            return Ok(title);
        }

        [HttpGet]
        [Route("rating")]
        public IActionResult GetRatingForTitle([FromBody] TitleDTO TitleRating)
        {
            var result = _dataService.GetRatingForTitle(TitleRating.TitleId);

            return Ok(result);
        }

        [HttpGet]
        [Route("genre")]
        public IActionResult GetGenres()
        {
            var result = _dataService.GetGenres();

            return Ok(result);
        }



        [HttpPost]
        [Route("ratetitle")]
        public IActionResult RateTitle([FromBody] UserTitleRating urt)
        {
            if (_dataService.RateTitle(urt))
            {
                return Ok(urt);
            }
            return BadRequest();


        }

        string IOurcontroller<Title>.GetUrl(Title obj)
        {
            return _linkGenerator.GetUriByName(HttpContext, nameof(GetTitle), new { obj.TitleId });
        }

        private string GetTopTitlesUrl(int page, int pageSize, string orderBy)
        {
            return _linkGenerator.GetUriByName(
                HttpContext,
                nameof(GetTitles),
                new { page, pageSize, orderBy });
        }
    }
}
