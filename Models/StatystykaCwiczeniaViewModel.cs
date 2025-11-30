namespace BeFit.Models
{
    public class StatystykaCwiczeniaViewModel
    {
        public int TypCwiczeniaId { get; set; }
        public string TypCwiczeniaNazwa { get; set; } = string.Empty;
        public int LiczbaWykonan { get; set; }
        public int LacznaLiczbaPowtorzen { get; set; }
        public double SrednieObciazenie { get; set; }
        public double MaksymalneObciazenie { get; set; }
    }
}