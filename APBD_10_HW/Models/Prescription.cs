using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_10_HW.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int IdPatient { get; set; }
        [Required]
        public int IdDoctor { get; set; }
        [ForeignKey("IdPatient")]
        public Patient Patient { get; set; } = null!;
        [ForeignKey("IdDoctor")]
        public Doctor Doctor { get; set; } = null!;
        public ICollection<PrescriptionMedication> PrescriptionMedications { get; set; } = new List<PrescriptionMedication>();
    }
} 