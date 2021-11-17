using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMDBdataservice;
using IMDBdataservice.Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        IbaseService _dataService;

        //My functions
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

        [HttpGet]
        [Route("search/person")] //Weird route aagain
        public IActionResult SearchPersons([FromBody] Person person, [FromQuery] IMDBdataservice.QueryStringOur queryString)
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
