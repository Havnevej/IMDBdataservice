using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMDBdataservice;
using IMDBdataservice.Service;
using WebServiceToken.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase, IOurcontroller<Person>
    {
        IbaseService _dataService;
        LinkGenerator _linkGenerator;

        public PersonController(ILogger<PersonController> logger, IbaseService dataService, LinkGenerator linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddPerson([FromBody] Person p)
        {
            if (_dataService.AddPerson(p))
            {
                return CreatedAtRoute(null, p);
            }
            else
            {
                return BadRequest(new{Data = "not Created"});
            }
        }

        [HttpDelete]
        [Route("remove/{personId}")]
        public IActionResult RemovePerson(string personId)
        {
            if (_dataService.RemovePerson(personId))
            {
                return Ok("{\"message\":\"removed person\"}"); //"{"message":"removed person"}"
            }
            else
            {
                return BadRequest(new { Data = "Already removed" });
            }
        }

        [HttpPost] // TODO: Add More error handling
        [Route("bookmark")]
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

        [HttpGet]
        [Route("search")]
        public IActionResult SearchPersons([FromBody] Person person, [FromQuery] QueryStringOur queryString)
        {
            var result = _dataService.SearchPersons(person, queryString);
            if (result.Count == 0)
            {
                return Ok("{\"message\":\"No results found\"}");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")] 
        public IActionResult GetPerson(string id)
        {
            var title = _dataService.GetPerson(id);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }

        [HttpPut]
        public IActionResult UpdatePerson([FromBody] PersonDTO person)
        {
            
            if (!_dataService.GetImdbContext().People.Any(x => x.PersonId == person.PersonId))
            {
                return BadRequest(new { Message = $"Personid{person.PersonId} does not exits" });
            }
            if (_dataService.UpdatePerson(person))
            {
                return Ok(new { Message = $"Updated person:{person.PersonId}" });
            }
            return BadRequest(new { Message = $"unknown error", Data = person });

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

        public string GetUrl(Person obj)
        {
            throw new NotImplementedException();
        }

        #region automatic controllers
        /*
        //private readonly imdbContext _context;

        public PersonController(imdbContext context)
       {
           _context = context;
       }
       */

        /*
        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.ToListAsync();
            
            var linkBuilder = new PageLinkBuilder(Url, "", null, queryString.Page, queryString.PageSize, total);
            DataWithPaging re = new(genre.Result, linkBuilder);
            
        }
        */

        /*
        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(string id)
        {
            var person = await _dataService.GetPerson(id).FindAsync();

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }
        */
        /*
        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(string id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */
        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonExists(person.PersonId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
        }
        */

        /*
        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(string id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(string id)
        {
            return _context.People.Any(e => e.PersonId == id);
        }
        */

        #endregion
    }
}
