using System.ComponentModel.DataAnnotations;

namespace APBD_10_HW.Models.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int IdDoctor { get; set; }
        [Required]
        public PatientDto Patient { get; set; } = null!;
        [Required]
        public List<MedicationDto> Medications { get; set; } = new List<MedicationDto>();
    }
} 