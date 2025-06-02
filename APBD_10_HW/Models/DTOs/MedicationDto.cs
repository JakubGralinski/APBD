using System.ComponentModel.DataAnnotations;

namespace APBD_10_HW.Models.DTOs
{
    public class MedicationDto
    {
        [Required]
        public int IdMedication { get; set; }
        public int? Dose { get; set; }
        [MaxLength(100)]
        public string? Details { get; set; }
    }
} 