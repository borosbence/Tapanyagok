using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tapanyagok.Server.DTOs;
using Tapanyagok.Server.Models;

namespace Tapanyagok.Server.Controllers
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

        // POST: api/Tapanyagok/server-side
        [HttpPost]
        [Route("server-side")]
        public async Task<IActionResult> Post([FromForm] DTPostModel model)
        {
            if (model.Draw == 0)
            {
                return BadRequest();
            }

            var query = _context.tapanyagok.AsQueryable();

            // Összes rekord kiszámítása
            int totalRecords = await query.CountAsync();

            // Keresés
            string? searchKey = model?.Search?.Value;
            // Szűrt rekordok száma
            int filteredCount = 0;

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                searchKey = searchKey.Replace('.', ',');
                query = query.Where(x => x.nev.Contains(searchKey) ||
                                        x.energia.ToString().Contains(searchKey) ||
                                        x.feherje.ToString().Contains(searchKey) ||
                                        x.szenhidrat.ToString().Contains(searchKey) ||
                                        x.zsir.ToString().Contains(searchKey));
                filteredCount = await query.CountAsync();
            }

            // Sorba rendezés
            string? sortKey = model.Columns[model.Order[0].Column].Data;
            string? sortDirection = model?.Order?[0].Dir;
            if (!string.IsNullOrEmpty(sortKey))
            {
                bool asc = sortDirection == "desc" ? false : true;
                switch (sortKey)
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
            query = query.Skip(model.Start).Take(model.Length);

            // Eredmény lekérdezése memóriába
            List<Tapanyag> result = await query.ToListAsync();

            // Szűrt elemek kiszámítása
            // Ha nincs keresés akkor minden rekord megjelenik,
            // ha van akkor pedig a keresésben lévő, de nem megjelent (oldalszám szerinti) rekordokat kell visszaadni
            int filteredRecords = string.IsNullOrWhiteSpace(searchKey) ? totalRecords : filteredCount;

            // Válasz készítése
            DTResult<Tapanyag> response = new()
            {
                Draw = model.Draw,
                RecordsFiltered = filteredRecords,
                RecordsTotal = totalRecords,
                Data = result
            };
            return Ok(response);
        }

        // GET: api/Tapanyagok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tapanyag>>> Gettapanyagok()
        {
            return await _context.tapanyagok.ToListAsync();
        }

        // GET: api/Tapanyagok/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tapanyag>> GetTapanyag(int id)
        {
            var tapanyag = await _context.tapanyagok.FindAsync(id);

            if (tapanyag == null)
            {
                return NotFound();
            }

            return tapanyag;
        }

        // PUT: api/Tapanyagok/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTapanyag(int id, Tapanyag tapanyag)
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

        // POST: api/Tapanyagok
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tapanyag>> PostTapanyag(Tapanyag tapanyag)
        {
            _context.tapanyagok.Add(tapanyag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTapanyag", new { id = tapanyag.id }, tapanyag);
        }

        // DELETE: api/Tapanyagok/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTapanyag(int id)
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
