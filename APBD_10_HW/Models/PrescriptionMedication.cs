using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_10_HW.Models
{
    public class PrescriptionMedication
    {
        [Key]
        public int IdPrescription { get; set; }
        [Key]
        public int IdMedication { get; set; }
        public int? Dose { get; set; }
        [MaxLength(100)]
        public string? Details { get; set; }
        [ForeignKey("IdPrescription")]
        public Prescription Prescription { get; set; } = null!;
        [ForeignKey("IdMedication")]
        public Medication Medication { get; set; } = null!;
    }
} 