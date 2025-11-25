using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Sesja
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rozpoczęcie")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Zakończenie")]
        public DateTime Koniec { get; set; }

        // Nawigacja - lista ćwiczeń w sesji
        public virtual ICollection<Cwiczenie>? Cwiczenia { get; set; }
    }
}