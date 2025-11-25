using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{

    [Authorize]
    public class SesjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Sesja.ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null) return NotFound();

            var sesja = await _context.Sesja.FirstOrDefaultAsync(s => s.Id == id);

            if (sesja == null) return NotFound();

            return View(sesja);
        }

        public IActionResult Utworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utworz(Sesja model)
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

            var sesja = await _context.Sesja.FindAsync(id);
            if (sesja == null) return NotFound();

            return View(sesja);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, Sesja model)
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

            var sesja = await _context.Sesja.FirstOrDefaultAsync(s => s.Id == id);
            if (sesja == null) return NotFound();

            return View(sesja);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var sesja = await _context.Sesja.FindAsync(id);
            _context.Sesja.Remove(sesja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}