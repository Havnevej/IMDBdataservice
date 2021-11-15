using Microsoft.AspNetCore.Mvc;
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
        [Route("search/primary")] // Kinda works but not really, works with https://localhost:5001/api/titles/search/primary?Page=5&PrimaryTitle=James but not James Bond
        public IActionResult SearchTitles([FromQuery] QueryString queryString) {
            var primaryTitle = _dataService.SearchTitles(queryString);
            return Ok(primaryTitle);
        }

        [HttpPost] // TODO: Add More error handling
        public IActionResult BookmarkTitle([FromBody] BookmarkTitle bt) 
        {
            Console.WriteLine(bt.TitleId);
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
        public IActionResult GetCommentsByTitleId(string id, [FromQuery] QueryString queryString)
        {
            var result = _dataService.GetCommentsByTitleId(id, queryString);
            if (result.Count == 0)
            {
                return Ok("{\"message\":\"No comments\"}");
            }
            return Ok(result);
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
