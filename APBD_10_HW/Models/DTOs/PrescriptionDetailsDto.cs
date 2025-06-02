namespace APBD_10_HW.Models.DTOs
{
    public class PrescriptionDetailsDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; } = null!;
        public List<MedicationDetailsDto> Medications { get; set; } = new List<MedicationDetailsDto>();
    }
} 