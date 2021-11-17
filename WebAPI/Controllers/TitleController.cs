using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using IMDBdataservice.Service;
using IMDBdataservice;
using Microsoft.AspNetCore.Routing;
using WebServiceToken.Attributes;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebServiceToken.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/titles")]
    public class TitleController : ControllerBase, IOurcontroller<Title>
    {
        IbaseService _dataService;
        LinkGenerator _linkGenerator;
        public TitleController(ILogger<TitleController> logger, IbaseService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
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
        [HttpGet]
        public IActionResult GetTitles([FromQuery] QueryStringOur queryString) //not implemented ?depth=100&?page=2 example
        { //temp, needs to be in parameter
            var title = _dataService.GetTopTitles(queryString);

            if (title == null)
            {
                return NotFound();
            }

            long total = _dataService.GetImdbContext().Titles.Count();
            var linkBuilder = new PageLinkBuilder(Url, "", null, queryString.Page, queryString.PageSize, total);
            DataWithPaging re = new(title.Result, linkBuilder);

            return Ok(re);
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
                total = _dataService.GetImdbContext().Titles.Include(x => x.Genres).Include(x => x.TitleRating).
                    Where(x => x.Genres.Any(x => x.GenreName == queryString.Genre)).ToList().Count;
            } else {
                result = _dataService.SearchTitleByGenre(queryString).Result;
                total = _dataService.GetImdbContext().Titles.Include(x => x.Genres).Include(x => x.TitleRating).
                    Where(x => x.Genres.Any(x => x.GenreName == queryString.Genre)).ToList().Count;
            }
            
            if (result.Count == 0)
            {
                return NotFound();
            }
            
            var linkBuilder = new PageLinkBuilder(Url, "", null, queryString.Page, queryString.PageSize, total);
            DataWithPaging re = new(result, linkBuilder);

            return Ok(re);
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

            return Ok(history);
         
            
        }

        [HttpGet]
        [Route("freq")]
        public IActionResult GetMostFrequentPerson([FromQuery] QueryStringOur queryString)
        {
            var result = _dataService.GetMostFrequentPerson(queryString);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.Select(cc));
        }

        private Freq cc(Person person)
        {
            return new Freq
            {
                name = person.PersonName
            };
        }


        [HttpPost]
        [Route("remove")]
        public IActionResult RemoveTitle([FromBody] Title t)
        {
            if (_dataService.RemoveTitle(t))
            {
                return Ok("removed");
            }
            else
            {
                return BadRequest("Already removed");
            }
        }

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

        [HttpPost] // TODO: Add More error handling
        [Route("comments")] 
        public IActionResult CommentTitle([FromBody] Comment comment)
        {
            _dataService.CommentTitle(comment);
            return Ok();
        }

        [HttpGet]
        [Route("comments/{id}")] 
        public IActionResult GetCommentsByTitleId(string id, [FromQuery] QueryStringOur queryString)
        {
            var result = _dataService.GetCommentsByTitleId(id, queryString);
            if (result.Count == 0)
            {
                return Ok("{\"message\":\"No comments\"}");
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateTitle(string id, [FromBody] TitleDTO title)
        {
            title.TitleId = id;
            if (!_dataService.GetImdbContext().Titles.Any(x => x.TitleId == id))
            {
                return BadRequest("Id does not exits");
            }
            return Ok(_dataService.UpdateTitle(title));

        }


        [HttpPut]
        [Route("person/{id}")]
        public IActionResult UpdatePerson(string id, [FromBody] PersonDTO person)
        {
            person.PersonId = id;
            if (!_dataService.GetImdbContext().People.Any(x => x.PersonId == id))
            {
                return BadRequest("Id does not exits");
            }
            return Ok(_dataService.UpdatePerson(person));

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
