using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tapanyagok.API.Models;

namespace Tapanyagok.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TapanyagokController : ControllerBase
    {
        private readonly TapanyagContext _context;

        public TapanyagokController(TapanyagContext context)
        {
            _context = context;
        }

        // GET: api/Tapanyagok/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tapanyag>>> Get()
        {
            return await _context.tapanyagok.ToListAsync();
        }

        // GET: api/Tapanyagok/jqGrid
        [HttpGet]
        [Route("jqGrid")]
        public async Task<IActionResult> Get(bool _search = false,
            int rows = 10, int page = 1,
            string sidx = null, string sord = null,
            string searchField = null, string searchString = null, string searchOper = null)
        {
            //var query = await _context.tapanyagok.ToListAsync() as IEnumerable<Tapanyag>;
            var query = _context.tapanyagok.AsQueryable();
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            // Keresés
            if (_search)
            {
                decimal.TryParse(searchString, out decimal searchNumber);
                searchString = searchString.Replace('.', ',');
                switch (searchField)
                {
                    case "nev":
                        switch (searchOper)
                        {
                            case "eq":
                                query = query.Where(x => x.nev.Equals(searchString));
                                break;
                            case "in":
                                query = query.Where(x => x.nev.Contains(searchString));
                                break;
                            default:
                                break;
                        }
                        break;
                    case "energia":
                        switch (searchOper)
                        {
                            case "eq":
                                query = query.Where(x => x.energia == searchNumber);
                                break;
                            case "in":
                                query = query.Where(x => x.energia.ToString().Contains(searchString));
                                break;
                            default:
                                break;
                        }
                        break;
                    case "feherje":
                        switch (searchOper)
                        {
                            case "eq":
                                query = query.Where(x => x.feherje == searchNumber);
                                break;
                            case "in":
                                query = query.Where(x => x.feherje.ToString().Contains(searchString));
                                break;
                            default:
                                break;
                        }
                        break;
                    case "zsir":
                        switch (searchOper)
                        {
                            case "eq":
                                query = query.Where(x => x.zsir == searchNumber);
                                break;
                            case "in":
                                query = query.Where(x => x.zsir.ToString().Contains(searchString));
                                break;
                            default:
                                break;
                        }
                        break;
                    case "szenhidrat":
                        switch (searchOper)
                        {
                            case "eq":
                                query = query.Where(x => x.szenhidrat == searchNumber);
                                break;
                            case "in":
                                query = query.Where(x => x.szenhidrat.ToString().Contains(searchString));
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            // Sorba rendezés
            if (!string.IsNullOrEmpty(sidx))
            {
                bool asc = sord == "asc" ? true : false;
                switch (sidx)
                {
                    case "nev":
                        query = asc ? query.OrderBy(x => x.nev) : query.OrderByDescending(x => x.nev);
                        break;
                    case "energia":
                        query = asc ? query.OrderBy(x => x.energia) : query.OrderByDescending(x => x.energia);
                        break;
                    case "feherje":
                        query = asc ? query.OrderBy(x => x.feherje) : query.OrderByDescending(x => x.feherje);
                        break;
                    case "zsir":
                        query = asc ? query.OrderBy(x => x.zsir) : query.OrderByDescending(x => x.zsir);
                        break;
                    case "szenhidrat":
                        query = asc ? query.OrderBy(x => x.szenhidrat) : query.OrderByDescending(x => x.szenhidrat);
                        break;
                    default:
                        break;
                }
            }

            // Oldaltördelés
            //var result = query.Skip(pageIndex * pageSize).Take(pageSize);

            var result = await query.ToListAsync() as IEnumerable<Tapanyag>;
            result = result.Skip(pageIndex * pageSize).Take(pageSize);
            return Ok(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = result,
            });
        }

        // GET: api/Tapanyags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tapanyag>> Get(int id)
        {
            var tapanyag = await _context.tapanyagok.FindAsync(id);

            if (tapanyag == null)
            {
                return NotFound();
            }

            return tapanyag;
        }

        // PUT: api/Tapanyags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Tapanyag tapanyag)
        {
            if (id != tapanyag.id)
            {
                return BadRequest();
            }

            _context.Entry(tapanyag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TapanyagExists(id))
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

        // POST: api/Tapanyags
        [HttpPost]
        public async Task<ActionResult<Tapanyag>> Post(Tapanyag tapanyag)
        {
            _context.tapanyagok.Add(tapanyag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = tapanyag.id }, tapanyag);
        }

        // DELETE: api/Tapanyags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tapanyag = await _context.tapanyagok.FindAsync(id);
            if (tapanyag == null)
            {
                return NotFound();
            }

            _context.tapanyagok.Remove(tapanyag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TapanyagExists(int id)
        {
            return _context.tapanyagok.Any(e => e.id == id);
        }

    }
}
