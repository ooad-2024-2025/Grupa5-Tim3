using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Grupa5Tim3.servis;

namespace Grupa5Tim3.Controllers
{
    [Authorize]
    public class AukcijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AukcijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aukcija
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aukcija.ToListAsync());
        }

        // GET: Aukcija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aukcija = await _context.Aukcija
                .FirstOrDefaultAsync(m => m.AukcijaID == id);
            if (aukcija == null)
            {
                return NotFound();
            }

            return View(aukcija);
        }

        // GET: Aukcija/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aukcija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Aukcija/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AukcijaID,umjetninaID,trenutnaCijena,pocetakAukcije,zavrsetakAukcije,status")] Aukcija aukcija)
        {
            if (ModelState.IsValid)
            {
              
                var userId = _context.Users
                    .FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;

                if (userId == null)
                {
                    return Forbid(); 
                }

                aukcija.kupacID = userId;

                _context.Add(aukcija);
                await _context.SaveChangesAsync();
                BackgroundJob.Schedule<AukcijaService>(
                    service => service.ZatvoriAukciju(aukcija.AukcijaID),
                    aukcija.zavrsetakAukcije - DateTime.Now);

                return RedirectToAction(nameof(Index));
            }
            return View(aukcija);
        }

        // GET: Aukcija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aukcija = await _context.Aukcija.FindAsync(id);
            if (aukcija == null)
            {
                return NotFound();
            }
            return View(aukcija);
        }

        // POST: Aukcija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AukcijaID,umjetninaID,trenutnaCijena,pocetakAukcije,zavrsetakAukcije,status")] Aukcija aukcija)
        {
            if (id != aukcija.AukcijaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var postojećaAukcija = await _context.Aukcija.FindAsync(id);
                    if (postojećaAukcija == null)
                    {
                        return NotFound();
                    }

                    postojećaAukcija.umjetninaID = aukcija.umjetninaID;
                    postojećaAukcija.trenutnaCijena = aukcija.trenutnaCijena;
                    postojećaAukcija.pocetakAukcije = aukcija.pocetakAukcije;
                    postojećaAukcija.zavrsetakAukcije = aukcija.zavrsetakAukcije;
                    postojećaAukcija.status = aukcija.status;
             

                    _context.Update(postojećaAukcija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AukcijaExists(aukcija.AukcijaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aukcija);
        }


        // GET: Aukcija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aukcija = await _context.Aukcija
                .FirstOrDefaultAsync(m => m.AukcijaID == id);
            if (aukcija == null)
            {
                return NotFound();
            }

            return View(aukcija);
        }

        // POST: Aukcija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aukcija = await _context.Aukcija.FindAsync(id);
            if (aukcija != null)
            {
                _context.Aukcija.Remove(aukcija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> Bid(int id)
        {
            var aukcija = await _context.Aukcija
                .FirstOrDefaultAsync(a => a.umjetninaID == id && a.status == Status.Aktivna);

            if (aukcija == null) return NotFound();

            return View(aukcija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Bid(int id, double novaCijena)
        {
            var aukcija = await _context.Aukcija.FindAsync(id);
            if (aukcija == null || aukcija.status !=Status.Aktivna) return NotFound();

            if (novaCijena <= aukcija.trenutnaCijena)
            {
                ModelState.AddModelError("", "Nova cijena mora biti veća od trenutne.");
                return View(aukcija);
            }

            var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            if (userId == null) return Forbid();

            aukcija.trenutnaCijena = novaCijena;
            aukcija.kupacID = userId;

            _context.Update(aukcija);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = aukcija.AukcijaID });
        }

        private bool AukcijaExists(int id)
        {
            return _context.Aukcija.Any(e => e.AukcijaID == id);
        }
    }
}
