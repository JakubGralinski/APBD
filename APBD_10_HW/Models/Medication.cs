using System.ComponentModel.DataAnnotations;

namespace APBD_10_HW.Models
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Type { get; set; } = null!;
        public ICollection<PrescriptionMedication> PrescriptionMedications { get; set; } = new List<PrescriptionMedication>();
    }
} 