using System.ComponentModel.DataAnnotations;

namespace APBD_10_HW.Models.DTOs
{
    public class PatientDto
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
    }
} 