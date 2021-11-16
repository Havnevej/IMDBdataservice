﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDBdataservice.Service;
using IMDBdataservice;
using Microsoft.AspNetCore.Routing;
using WebServiceToken.Attributes;

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
            var title = _dataService.GetTopTitles(queryString);
            if (title == null)
            {
                return NotFound();
            }

            return Ok(title.Result);
        }

        [HttpGet]
        [Route("search")]
        public IActionResult SearchTitleByGenre([FromQuery] QueryString queryString)
        {
            var genre = _dataService.SearchTitleByGenre(queryString);
            Console.WriteLine(queryString.Genre);
            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre.Result);
        }
        [Authorization]
        [HttpGet]
        [Route("gethist")]
        public IActionResult Get__search_history()
        {
            var history = _dataService.GetSearchHistory();
       
            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
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
