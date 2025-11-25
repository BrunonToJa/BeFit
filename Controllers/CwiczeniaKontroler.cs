using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        // -------------------- INDEX --------------------
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Cwiczenie
                .Include(c => c.Sesja)
                .Include(c => c.TypCwiczenia)
                .ToListAsync();

            return View(lista);
        }

        // -------------------- SZCZEGÓŁY --------------------
        public async Task<IActionResult> Szczegoly(int? id)
        {
            if (id == null) return NotFound();

            var cw = await _context.Cwiczenie
                .Include(c => c.Sesja)
                .Include(c => c.TypCwiczenia)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cw == null) return NotFound();

            return View(cw);
        }

        // -------------------- LISTY DO FORMULARZA --------------------
        private void PrzygotujListy(int? sesjaId = null, int? typId = null)
        {
            // Pobierz sesje i stwórz SelectList z odpowiednimi wartościami tekstowymi
            var sesje = _context.Sesja
                .OrderBy(s => s.Start)
                .Select(s => new { s.Id, DisplayText = s.Start.ToString("yyyy-MM-dd HH:mm") })
                .ToList();

            ViewBag.SesjaId = new SelectList(sesje, "Id", "DisplayText", sesjaId);

            // Pobierz typy ćwiczeń i użyj bezpośrednio właściwości Nazwa
            var typy = _context.TypCwiczenia
                .OrderBy(t => t.Nazwa)
                .ToList();

            ViewBag.TypCwiczeniaId = new SelectList(typy, "Id", "Nazwa", typId);
        }

        // -------------------- UTWÓRZ --------------------
        public IActionResult Utworz()
        {
            PrzygotujListy();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Utworz(Cwiczenie model)
        {
            if (ModelState.IsValid)
            {
                _context.Cwiczenie.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PrzygotujListy(model.SesjaId, model.TypCwiczeniaId);
            return View(model);
        }

        // -------------------- EDYTUJ --------------------
        public async Task<IActionResult> Edytuj(int? id)
        {
            if (id == null) return NotFound();

            var cw = await _context.Cwiczenie.FindAsync(id);
            if (cw == null) return NotFound();

            PrzygotujListy(cw.SesjaId, cw.TypCwiczeniaId);
            return View(cw);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, Cwiczenie model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Cwiczenie.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CwiczenieExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            PrzygotujListy(model.SesjaId, model.TypCwiczeniaId);
            return View(model);
        }

        // -------------------- USUŃ --------------------
        public async Task<IActionResult> Usun(int? id)
        {
            if (id == null) return NotFound();

            var cw = await _context.Cwiczenie
                .Include(c => c.Sesja)
                .Include(c => c.TypCwiczenia)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cw == null) return NotFound();

            return View(cw);
        }

        [HttpPost, ActionName("Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzenie(int id)
        {
            var cw = await _context.Cwiczenie.FindAsync(id);
            if (cw != null)
            {
                _context.Cwiczenie.Remove(cw);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // -------------------- POMOCNICZE --------------------
        private bool CwiczenieExists(int id)
        {
            return _context.Cwiczenie.Any(e => e.Id == id);
        }
    }
}