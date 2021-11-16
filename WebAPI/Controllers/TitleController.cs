﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using IMDBdataservice.Service;
using IMDBdataservice;
using Microsoft.AspNetCore.Routing;
using WebAPI.Helpers;

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
        public IActionResult GetTitles([FromQuery] QueryString queryString) //not implemented ?depth=100&?page=2 example
        { //temp, needs to be in parameter
            Console.WriteLine(queryString.Genre);

            var title = _dataService.GetTopTitles(queryString);

            // Get total number of records
            long total = _dataService.GetImdbContext().Titles.Count();

            var linkBuilder = new PageLinkBuilder(Url, "", null, queryString.Page, queryString.PageSize, total);

            if (title == null)
            {
                return NotFound();
            }

            DataWithPaging re = new(title.Result, linkBuilder);
            return Ok(re);
        }

        [HttpGet]
        [Route("search")]
        public IActionResult SearchTitleByGenre([FromQuery] QueryString queryString)
        {
            var genre = _dataService.SearchTitleByGenre(queryString);
            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre.Result);
        }

        [HttpGet]
        [Route("search/primary")]
        public IActionResult SearchTitles([FromBody] Title title,[FromQuery] QueryString queryString) {
            var result = _dataService.SearchTitles(title, queryString);
            if (result.Count == 0)
            {
                return Ok("{\"message\":\"No results found\"}");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTitle([FromBody] Title t) 
        {
            if (_dataService.AddTitle(t))
            {
               return Ok("inserted");
            }
            else
            {
                return BadRequest("Already exists");
            }
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

        [HttpPost]
        [Route("add/person")]
        public IActionResult AddPerson([FromBody] Person p) 
        {
            if (_dataService.AddPerson(p))
            {
               return Ok("inserted");
            }
            else
            {
                return BadRequest("Already exists");
            }
        }

        [HttpPost]
        [Route("remove/person")]
        public IActionResult RemovePerson([FromBody] Person p)
        {
            if (_dataService.RemovePerson(p))
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
        [Route("bookmark/person")]
        public IActionResult BookmarkPerson([FromBody] BookmarkPerson bp)
        {
            if (_dataService.BookmarkPerson(bp))
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
        public IActionResult GetCommentsByTitleId(string id, [FromQuery] QueryString queryString)
        {
            var result = _dataService.GetCommentsByTitleId(id, queryString);
            if (result.Count == 0)
            {
                return Ok("{\"message\":\"No comments\"}");
            }
            return Ok(result);
        }


        [HttpGet]
        [Route("search/person")] //Weird route aagain
        public IActionResult SearchPersons([FromBody] Person person, [FromQuery] QueryString queryString)
        {
            var result = _dataService.SearchPersons(person, queryString);
            if (result.Count == 0)
            {
                return Ok("{\"message\":\"No results found\"}");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("person/{id}")] //Route is weird, make person controller?
        public IActionResult GetPerson(string id)
        {
            var title = _dataService.GetPerson(id);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
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
