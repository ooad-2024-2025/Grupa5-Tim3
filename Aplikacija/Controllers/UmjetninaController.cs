using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Grupa5Tim3.servis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Umjetnina.ToListAsync());
        }

        // GET: Umjetninas/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Umjetninas/Create
        // GET: Umjetnina/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Umjetnina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

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
