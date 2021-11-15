using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDBdataservice.Service;
using IMDBdataservice;
using Microsoft.AspNetCore.Routing;

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
