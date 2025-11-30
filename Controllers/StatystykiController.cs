using System.Security.Claims;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatystykiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatystykiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var odKiedy = DateTime.UtcNow.AddDays(-28);

            var cwiczenia = await _context.Cwiczenie
                .Include(c => c.TypCwiczenia)
                .Include(c => c.Sesja)
                .Where(c => c.UzytkownikId == userId && c.Sesja!.Start >= odKiedy)
                .ToListAsync();

            var statystyki = cwiczenia
                .GroupBy(c => c.TypCwiczenia!)
                .Select(gr => new StatystykaCwiczeniaViewModel
                {
                    TypCwiczeniaId = gr.Key.Id,
                    TypCwiczeniaNazwa = gr.Key.Nazwa,
                    LiczbaWykonan = gr.Count(),
                    LacznaLiczbaPowtorzen = gr.Sum(x => x.Serie * x.Powtorzenia),
                    SrednieObciazenie = gr.Average(x => x.Ciezar),
                    MaksymalneObciazenie = gr.Max(x => x.Ciezar)
                })
                .OrderBy(s => s.TypCwiczeniaNazwa)
                .ToList();

            return View(statystyki);
        }
    }
}