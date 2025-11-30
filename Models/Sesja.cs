using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class Sesja : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Rozpoczęcie", Description = "Podaj datę i czas startu treningu")]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Zakończenie", Description = "Podaj datę i czas zakończenia treningu")]
        public DateTime Koniec { get; set; }

        [Required]
        [Display(Name = "Użytkownik")]
        public string UzytkownikId { get; set; } = string.Empty;
        public IdentityUser? Uzytkownik { get; set; }

        public virtual ICollection<Cwiczenie>? Cwiczenia { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Start == default)
            {
                yield return new ValidationResult("Musisz podać datę rozpoczęcia.", new[] { nameof(Start) });
            }

            if (Koniec == default)
            {
                yield return new ValidationResult("Musisz podać datę zakończenia.", new[] { nameof(Koniec) });
            }

            if (Start != default && Koniec != default && Koniec <= Start)
            {
                yield return new ValidationResult("Zakończenie musi być późniejsze niż rozpoczęcie.", new[] { nameof(Koniec) });
            }
        }
    }
}