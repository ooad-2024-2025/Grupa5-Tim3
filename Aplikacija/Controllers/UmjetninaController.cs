using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Grupa5Tim3.servis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grupa5Tim3.Controllers
{
    public class UmjetninaController : Controller
    {
    
        private readonly ApplicationDbContext _context;

        public UmjetninaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Umjetninas
        public async Task<IActionResult> Index(
       List<string> autori,
       List<string> tehnike,
       List<string> periodi,
       int? auctionStatusIndex,
       string search)
        {
            var query = _context.Umjetnina.AsQueryable();

            // Search tekst - pretraga po više polja
            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(u =>
                    u.naziv.ToLower().Contains(loweredSearch) ||
                    u.autor.ToLower().Contains(loweredSearch) ||
                    u.tehnika.ToLower().Contains(loweredSearch) ||
                    u.period.ToLower().Contains(loweredSearch));
            }

            // Filtriranje po autorima, tehnikama i periodima
            if (autori != null && autori.Any())
                query = query.Where(u => autori.Contains(u.autor));

            if (tehnike != null && tehnike.Any())
                query = query.Where(u => tehnike.Contains(u.tehnika));

            if (periodi != null && periodi.Any())
                query = query.Where(u => periodi.Contains(u.period));

            // Dobavljanje zadnje aukcije po umjetnini
            var aukcijeDict = await _context.Aukcija
                .GroupBy(a => a.umjetninaID)
                .Select(g => g.OrderByDescending(a => a.zavrsetakAukcije).FirstOrDefault())
                .ToDictionaryAsync(a => a.umjetninaID, a => a);

            // Auction status slider obrada
            var statusMap = new[] { "All", "Active", "Finalized", "Canceled", "Pending" };
            string selectedStatus = auctionStatusIndex.HasValue && auctionStatusIndex.Value >= 0 && auctionStatusIndex.Value < statusMap.Length
                ? statusMap[auctionStatusIndex.Value]
                : "All"; // defaultno na "All"

            if (selectedStatus != "All")
            {
                var statusMapping = new Dictionary<string, Status>(StringComparer.OrdinalIgnoreCase)
        {
            { "Active", Status.Aktivna },
            { "Finalized", Status.Finalizirana },
            { "Canceled", Status.Otkazana }
        };

                var validUmjetninaIds = new HashSet<int>();

                if (selectedStatus == "Pending")
                {
                    var umjetnineSaAukcijomIds = aukcijeDict.Keys.ToHashSet();

                    var umjetnineBezAukcije = await _context.Umjetnina
                        .Where(u => !umjetnineSaAukcijomIds.Contains(u.umjetinaID))
                        .Select(u => u.umjetinaID)
                        .ToListAsync();

                    validUmjetninaIds.UnionWith(umjetnineBezAukcije);
                }
                else if (statusMapping.TryGetValue(selectedStatus, out var statusEnum))
                {
                    validUmjetninaIds.UnionWith(
                        aukcijeDict
                            .Where(a => a.Value.status == statusEnum)
                            .Select(a => a.Key)
                    );
                }

                query = query.Where(u => validUmjetninaIds.Contains(u.umjetinaID));
            }

            var umjetnine = await query.ToListAsync();

            // ViewBag podaci
            ViewBag.Aukcije = aukcijeDict;
            ViewBag.Autori = await _context.Umjetnina.Select(u => u.autor).Distinct().ToListAsync();
            ViewBag.Tehnike = await _context.Umjetnina.Select(u => u.tehnika).Distinct().ToListAsync();
            ViewBag.Periodi = await _context.Umjetnina.Select(u => u.period).Distinct().ToListAsync();

            ViewBag.SelectedAutori = autori ?? new List<string>();
            ViewBag.SelectedTehnike = tehnike ?? new List<string>();
            ViewBag.SelectedPeriodi = periodi ?? new List<string>();
            ViewBag.SelectedAuctionStatusIndex = auctionStatusIndex;
            ViewBag.Search = search;

            return View(umjetnine);
        }


        // GET: Umjetninas/Details/5
        [Authorize(Policy = "ExcludeKriticar")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var umjetnina = await _context.Umjetnina.FindAsync(id);
            if (umjetnina == null)
                return NotFound();

            // Dohvati najnoviju aukciju za tu umjetninu (ako postoji)
            var aukcija = await _context.Aukcija
                .Where(a => a.umjetninaID == id)
                .OrderByDescending(a => a.zavrsetakAukcije)
                .FirstOrDefaultAsync();

            ViewBag.Aukcija = aukcija;

            return View(umjetnina);
        }
        // GET: Umjetninas/Create
        // GET: Umjetnina/Create
        [Authorize(Roles = "Administrator")]
        [Authorize(Policy = "ExcludeKriticar")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Umjetnina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ExcludeKriticar")]

        public async Task<IActionResult> Create([Bind("umjetinaID,naziv,autor,period,datum,tehnika,pocetnaCijena,opis")] Umjetnina umjetnina, IFormFile Slika)
        {
            if (ModelState.IsValid)
            {

                if (Slika != null && Slika.Length > 0)
                {
                    var fileName = Path.GetFileName(Slika.FileName);
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var filePath = Path.Combine(uploads, fileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Slika.CopyToAsync(stream);
                    }

                    umjetnina.SlikaPath = "/images/" + fileName;
                }

                _context.Add(umjetnina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(umjetnina);
        }


        // GET: Umjetninas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var umjetnina = await _context.Umjetnina.FindAsync(id);
            if (umjetnina == null)
            {
                return NotFound();
            }

            
            if (User.IsInRole("Kriticar") && umjetnina.pocetnaCijena != null && umjetnina.pocetnaCijena > 0)
            {
               
               return RedirectToAction("Index");
            }

            return View(umjetnina);
        }

        // POST: Umjetninas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("umjetinaID,naziv,autor,period,datum,tehnika,pocetnaCijena,opis")] Umjetnina umjetnina, IFormFile Slika)
        {
            if (id != umjetnina.umjetinaID)
                return NotFound();

            var emailSender = HttpContext.RequestServices.GetService<SendGridEmailSender>();

            if (User.IsInRole("Kriticar"))
            {
                ModelState.Remove("naziv");
                ModelState.Remove("autor");
                ModelState.Remove("period");
                ModelState.Remove("datum");
                ModelState.Remove("tehnika");
                ModelState.Remove("opis");
                ModelState.Remove("Slika");
            }
            else
            {
                if (Slika == null || Slika.Length == 0)
                    ModelState.Remove("Slika");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUmjetnina = await _context.Umjetnina.FindAsync(id);
                    if (existingUmjetnina == null)
                        return NotFound();

                    var user = HttpContext.User;

                    if (user.IsInRole("Kriticar"))
                    {
                        bool jeNovaCijena = umjetnina.pocetnaCijena.HasValue
                            && umjetnina.pocetnaCijena > 0
                            && (existingUmjetnina.pocetnaCijena != umjetnina.pocetnaCijena);
                        if (existingUmjetnina.pocetnaCijena != null && existingUmjetnina.pocetnaCijena > 0)
                        {
                           
                            ModelState.AddModelError(string.Empty, "Početna cijena je već postavljena i ne može se mijenjati.");
                            return View(umjetnina);
                        }

                        existingUmjetnina.pocetnaCijena = umjetnina.pocetnaCijena;

                        if (jeNovaCijena)
                        {
                            string subject = "Postavljena početna cijena";
                            string message = $"Kritičar je postavio početnu cijenu za umjetninu <strong>{existingUmjetnina.naziv}</strong> (ID: <strong>{existingUmjetnina.umjetinaID}</strong>): <strong>{umjetnina.pocetnaCijena} KM</strong>.";

                            if (emailSender != null)
                            {
                                await emailSender.SendEmailAsync("lamijabojic@gmail.com", subject, message);
                            }
                        }
                    }
                    else
                    {
                        existingUmjetnina.naziv = umjetnina.naziv;
                        existingUmjetnina.autor = umjetnina.autor;
                        existingUmjetnina.period = umjetnina.period;
                        existingUmjetnina.datum = umjetnina.datum;
                        existingUmjetnina.tehnika = umjetnina.tehnika;
                        existingUmjetnina.opis = umjetnina.opis;
                    }

                    if (Slika != null && Slika.Length > 0)
                    {
                        var fileName = Path.GetFileName(Slika.FileName);
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Slika.CopyToAsync(stream);
                        }

                        existingUmjetnina.SlikaPath = "/images/" + fileName;
                    }

                    _context.Update(existingUmjetnina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UmjetninaExists(umjetnina.umjetinaID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(umjetnina);
        }

        // GET: Umjetninas/Delete/5
        [Authorize(Policy = "ExcludeKriticar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var umjetnina = await _context.Umjetnina
                .FirstOrDefaultAsync(m => m.umjetinaID == id);
            if (umjetnina == null)
            {
                return NotFound();
            }

            return View(umjetnina);
        }

        // POST: Umjetninas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var umjetnina = await _context.Umjetnina.FindAsync(id);
            if (umjetnina != null)
            {
                _context.Umjetnina.Remove(umjetnina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UmjetninaExists(int id)
        {
            return _context.Umjetnina.Any(e => e.umjetinaID == id);
        }
    }
}
