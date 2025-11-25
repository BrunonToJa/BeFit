using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class TypCwiczenia
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Nazwa { get; set; }

        // Nawigacja - lista ćwiczeń tego typu
        public virtual ICollection<Cwiczenie>? Cwiczenia { get; set; }
    }
}
