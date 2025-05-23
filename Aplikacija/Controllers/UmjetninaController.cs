﻿using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;



namespace Grupa5Tim3.Controllers
{
    public class UmjetninaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly object uloga;

        public UmjetninaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Umjetnina
        public async Task<IActionResult> Index()
        {
            return View(await _context.Umjetnina.ToListAsync());
        }

        // GET: Umjetnina/Details/5
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

        // GET: Umjetnina/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Umjetnina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("umjetinaID,naziv,autor,period,datum,tehnika,pocetnaCijena")] Umjetnina umjetnina, IFormFile Slika)
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

        // GET: Umjetnina/Edit/5
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

        // POST: Umjetnina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("umjetinaID,naziv,autor,period,datum,tehnika,pocetnaCijena")] Umjetnina umjetnina, IFormFile NovaSlika)
        {
            if (id != umjetnina.umjetinaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var postojecaUmjetnina = await _context.Umjetnina.FindAsync(id);
                    if (postojecaUmjetnina == null)
                        return NotFound();

                    postojecaUmjetnina.naziv = umjetnina.naziv;
                    postojecaUmjetnina.autor = umjetnina.autor;
                    postojecaUmjetnina.period = umjetnina.period;
                    postojecaUmjetnina.datum = umjetnina.datum;
                    postojecaUmjetnina.tehnika = umjetnina.tehnika;
                    postojecaUmjetnina.pocetnaCijena = umjetnina.pocetnaCijena;

                   
                    if (NovaSlika != null && NovaSlika.Length > 0)
                    {
                        var fileName = Path.GetFileName(NovaSlika.FileName);
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await NovaSlika.CopyToAsync(stream);
                        }

                        postojecaUmjetnina.SlikaPath = "/images/" + fileName;
                    }

                    _context.Update(postojecaUmjetnina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UmjetninaExists(umjetnina.umjetinaID))
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

            return View(umjetnina);
        }


        // GET: Umjetnina/Delete/5
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

        // POST: Umjetnina/Delete/5
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
