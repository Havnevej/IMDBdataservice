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
    [Route("api/title")]
    public class TitleController : ControllerBase, IOurcontroller
    {
        readonly IbaseService _dataService;
        LinkGenerator _linkGenerator;

        public TitleController(ILogger<TitleController> logger)
        {

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

        public string GetUrl(object obj)
        {
            throw new NotImplementedException();
        }

        private string GetUrl(Title title)
        {
            return _linkGenerator.GetUriByName(HttpContext, nameof(GetTitle), new { title.TitleId });
        }
    }
}
