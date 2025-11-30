using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class Cwiczenie
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Sesja")]
        public int SesjaId { get; set; }
        public virtual Sesja? Sesja { get; set; }

        [Required]
        [Display(Name = "Typ ćwiczenia")]
        public int TypCwiczeniaId { get; set; }
        public virtual TypCwiczenia? TypCwiczenia { get; set; }

        [Required]
        [Range(1, 200, ErrorMessage = "Podaj liczbę serii w przedziale 1-200.")]
        [Display(Name = "Serie", Description = "Liczba serii wykonanych w trakcie ćwiczenia")]
        public int Serie { get; set; }

        [Required]
        [Range(1, 300, ErrorMessage = "Podaj liczbę powtórzeń w przedziale 1-300.")]
        [Display(Name = "Powtórzenia", Description = "Liczba powtórzeń w każdej serii")]
        public int Powtorzenia { get; set; }

        [Required]
        [Range(0, 500, ErrorMessage = "Obciążenie musi mieścić się w zakresie 0-500 kg.")]
        [Display(Name = "Obciążenie (kg)", Description = "Średnie obciążenie użyte w jednej serii")]
        public double Ciezar { get; set; }

        [Required]
        [Display(Name = "Użytkownik")]
        public string UzytkownikId { get; set; } = string.Empty;
        public IdentityUser? Uzytkownik { get; set; }
    }
}