using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class TypCwiczenia
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Nazwa { get; set; } = string.Empty;

        public virtual ICollection<Cwiczenie>? Cwiczenia { get; set; }
    }
}