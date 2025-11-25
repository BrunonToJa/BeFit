using System.ComponentModel.DataAnnotations;

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
        [Range(1, 200)]
        [Display(Name = "Serie")]
        public int Serie { get; set; }

        [Required]
        [Range(1, 300)]
        [Display(Name = "Powtórzenia")]
        public int Powtorzenia { get; set; }

        [Required]
        [Range(0, 500)]
        [Display(Name = "Obciążenie (kg)")]
        public double Ciezar { get; set; }
    }
}