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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var lista = await _context.TypCwiczenia.ToListAsync();
            return View(lista);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null) return NotFound();

            var typ = await _context.TypCwiczenia
                .FirstOrDefaultAsync(t => t.Id == id);

            if (typ == null) return NotFound();

            return View(typ);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Utworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null) return NotFound();

            var typ = await _context.TypCwiczenia.FindAsync(id);
            if (typ == null) return NotFound();

            return View(typ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var typ = await _context.TypCwiczenia.FindAsync(id);
            if (typ != null)
            {
                _context.TypCwiczenia.Remove(typ);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}