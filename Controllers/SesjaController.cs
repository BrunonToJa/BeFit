using BeFit.Data;
using System.Security.Claims;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private string PobierzUzytkownikId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public async Task<IActionResult> Index()
        {
            var userId = PobierzUzytkownikId();
            var lista = await _context.Sesja
                .Where(s => s.UzytkownikId == userId)
                .OrderByDescending(s => s.Start)
                .ToListAsync();

            return View(lista);
        }

        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null) return NotFound();

            var userId = PobierzUzytkownikId();
            var sesja = await _context.Sesja
                .Include(s => s.Cwiczenia)
                    .ThenInclude(c => c.TypCwiczenia)
                .FirstOrDefaultAsync(s => s.Id == id && s.UzytkownikId == userId);

            if (sesja == null) return NotFound();

            return View(sesja);
        }

        public IActionResult Utworz()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utworz([Bind("Start,Koniec")] Sesja model)
        {
            var userId = PobierzUzytkownikId();

            if (ModelState.IsValid)
            {
                model.UzytkownikId = userId;
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null) return NotFound();

            var userId = PobierzUzytkownikId();
            var sesja = await _context.Sesja.FirstOrDefaultAsync(s => s.Id == id && s.UzytkownikId == userId);
            if (sesja == null) return NotFound();

            return View(sesja);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, [Bind("Id,Start,Koniec")] Sesja model)
        {
            if (id != model.Id) return NotFound();

            var userId = PobierzUzytkownikId();
            var sesja = await _context.Sesja.FirstOrDefaultAsync(s => s.Id == id && s.UzytkownikId == userId);
            if (sesja == null) return NotFound();

            if (ModelState.IsValid)
            {
                sesja.Start = model.Start;
                sesja.Koniec = model.Koniec;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sesja);
        }

        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null) return NotFound();

            var userId = PobierzUzytkownikId();
            var sesja = await _context.Sesja
                .FirstOrDefaultAsync(s => s.Id == id && s.UzytkownikId == userId);

            if (sesja == null) return NotFound();

            return View(sesja);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var userId = PobierzUzytkownikId();
            var sesja = await _context.Sesja.FirstOrDefaultAsync(s => s.Id == id && s.UzytkownikId == userId);
            if (sesja != null)
            {
                _context.Sesja.Remove(sesja);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}