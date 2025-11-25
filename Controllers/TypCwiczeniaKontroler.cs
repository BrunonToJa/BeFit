using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [AllowAnonymous]
    public class TypCwiczeniaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypCwiczeniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.TypCwiczenia.ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null) return NotFound();

            var typ = await _context.TypCwiczenia
                .FirstOrDefaultAsync(t => t.Id == id);

            if (typ == null) return NotFound();

            return View(typ);
        }

        public IActionResult Utworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utworz(TypCwiczenia model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null) return NotFound();

            var typ = await _context.TypCwiczenia.FindAsync(id);
            if (typ == null) return NotFound();

            return View(typ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, TypCwiczenia model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null) return NotFound();

            var typ = await _context.TypCwiczenia
                .FirstOrDefaultAsync(t => t.Id == id);

            if (typ == null) return NotFound();

            return View(typ);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var typ = await _context.TypCwiczenia.FindAsync(id);
            _context.TypCwiczenia.Remove(typ);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}