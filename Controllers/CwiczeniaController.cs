using BeFit.Data;
using System.Security.Claims;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class CwiczeniaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CwiczeniaController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string PobierzUzytkownikId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        // -------------------- INDEX --------------------
        public async Task<IActionResult> Index()
        {
            var userId = PobierzUzytkownikId();
            var lista = await _context.Cwiczenie
                .Include(c => c.Sesja)
                .Include(c => c.TypCwiczenia)
                .Where(c => c.UzytkownikId == userId)
                .OrderByDescending(c => c.Sesja!.Start)
                .ToListAsync();

            return View(lista);
        }

        // -------------------- SZCZEGÓŁY --------------------
        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null) return NotFound();

            var userId = PobierzUzytkownikId();
            var cw = await _context.Cwiczenie
                .Include(c => c.Sesja)
                .Include(c => c.TypCwiczenia)
                .FirstOrDefaultAsync(m => m.Id == id && m.UzytkownikId == userId);

            if (cw == null) return NotFound();

            return View(cw);
        }

        // -------------------- LISTY DO FORMULARZA --------------------
        private void PrzygotujListy(string userId, int? sesjaId = null, int? typId = null)
        {
            var sesje = _context.Sesja
                .Where(s => s.UzytkownikId == userId)
                .OrderByDescending(s => s.Start)
                .Select(s => new { s.Id, DisplayText = s.Start.ToString("yyyy-MM-dd HH:mm") })
                .ToList();

            ViewBag.SesjaId = new SelectList(sesje, "Id", "DisplayText", sesjaId);

            var typy = _context.TypCwiczenia
                .OrderBy(t => t.Nazwa)
                .ToList();

            ViewBag.TypCwiczeniaId = new SelectList(typy, "Id", "Nazwa", typId);
        }

        // -------------------- UTWÓRZ --------------------
        public IActionResult Utworz()
        {
            var userId = PobierzUzytkownikId();
            PrzygotujListy(userId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utworz([Bind("SesjaId,TypCwiczeniaId,Serie,Powtorzenia,Ciezar")] Cwiczenie model)
        {
            var userId = PobierzUzytkownikId();

            var sesjaNalezyDoUzytkownika = await _context.Sesja.AnyAsync(s => s.Id == model.SesjaId && s.UzytkownikId == userId);
            if (!sesjaNalezyDoUzytkownika)
            {
                ModelState.AddModelError(nameof(Cwiczenie.SesjaId), "Możesz wybrać tylko swoje sesje treningowe.");
            }

            if (ModelState.IsValid)
            {
                model.UzytkownikId = userId;
                _context.Cwiczenie.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PrzygotujListy(userId, model.SesjaId, model.TypCwiczeniaId);
            return View(model);
        }

        // -------------------- EDYTUJ --------------------
        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null) return NotFound();

            var userId = PobierzUzytkownikId();
            var cw = await _context.Cwiczenie.FirstOrDefaultAsync(c => c.Id == id && c.UzytkownikId == userId);
            if (cw == null) return NotFound();

            PrzygotujListy(userId, cw.SesjaId, cw.TypCwiczeniaId);
            return View(cw);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, [Bind("Id,SesjaId,TypCwiczeniaId,Serie,Powtorzenia,Ciezar")] Cwiczenie model)
        {
            if (id != model.Id) return NotFound();

            var userId = PobierzUzytkownikId();
            var cw = await _context.Cwiczenie.FirstOrDefaultAsync(c => c.Id == id && c.UzytkownikId == userId);
            if (cw == null) return NotFound();

            var sesjaNalezyDoUzytkownika = await _context.Sesja.AnyAsync(s => s.Id == model.SesjaId && s.UzytkownikId == userId);
            if (!sesjaNalezyDoUzytkownika)
            {
                ModelState.AddModelError(nameof(Cwiczenie.SesjaId), "Możesz wybrać tylko swoje sesje treningowe.");
            }

            if (ModelState.IsValid)
            {
                cw.SesjaId = model.SesjaId;
                cw.TypCwiczeniaId = model.TypCwiczeniaId;
                cw.Serie = model.Serie;
                cw.Powtorzenia = model.Powtorzenia;
                cw.Ciezar = model.Ciezar;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PrzygotujListy(userId, model.SesjaId, model.TypCwiczeniaId);
            return View(cw);
        }

        // -------------------- USUŃ --------------------
        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null) return NotFound();

            var userId = PobierzUzytkownikId();
            var cw = await _context.Cwiczenie
                .Include(c => c.Sesja)
                .Include(c => c.TypCwiczenia)
                .FirstOrDefaultAsync(m => m.Id == id && m.UzytkownikId == userId);

            if (cw == null) return NotFound();

            return View(cw);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var userId = PobierzUzytkownikId();
            var cw = await _context.Cwiczenie.FirstOrDefaultAsync(c => c.Id == id && c.UzytkownikId == userId);
            if (cw != null)
            {
                _context.Cwiczenie.Remove(cw);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}