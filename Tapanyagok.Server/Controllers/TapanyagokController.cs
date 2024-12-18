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

        // GET: api/Tapanyagok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tapanyag>>> Gettapanyagok()
        {
            return await _context.tapanyagok.ToListAsync();
        }

        // POST: api/Tapanyagok/server-side
        [HttpPost]
        [Route("server-side")]
        public async Task<DTResult<Tapanyag>> Post([FromForm] DTPostModel model)
        {
            var query = _context.tapanyagok.AsQueryable();

            // Összes rekord kiszámítása
            int totalRecords = await query.CountAsync();

            // Keresés
            string? searchKey = model?.Search?.Value;
            // Szűrt rekordok száma
            // Ha nincs keresés akkor minden rekord megjelenik,
            // ha van akkor pedig a keresésben lévő, de nem megjelent (oldalszám szerinti) rekordokat kell visszaadni
            int filteredRecords = totalRecords;
            // Ha a keresési kulcsszónak van értéke, akkor indítson keresést
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                searchKey = searchKey.Replace('.', ',');
                query = query.Where(x => x.nev.Contains(searchKey) ||
                                        x.energia.ToString().Contains(searchKey) ||
                                        x.feherje.ToString().Contains(searchKey) ||
                                        x.szenhidrat.ToString().Contains(searchKey) ||
                                        x.zsir.ToString().Contains(searchKey));
                filteredRecords = await query.CountAsync();
            }

            // Rendezés
            string? sortKey = null;
            string? sortDirection = null;
            if (model.Order?.Count > 0) // 3. klikk javítás
            {
                sortKey = model.Columns[model.Order[0].Column].Data; // oszlop neve
                sortDirection = model.Order[0].Dir; // "asc" || "desc"
            }
            // Ha van a rendezési kulcsszónak értéke  
            if (!string.IsNullOrEmpty(sortKey))
            {
                bool asc = sortDirection == "asc";
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
            List<Tapanyag> data = await query.ToListAsync();

            // Válasz készítése
            DTResult<Tapanyag> result = new()
            {
                Draw = model.Draw,
                RecordsFiltered = filteredRecords,
                RecordsTotal = totalRecords,
                Data = data
            };
            return result;
        }
    }
}
