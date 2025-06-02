namespace APBD_10_HW.Models.DTOs
{
    public class PatientDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public List<PrescriptionDetailsDto> Prescriptions { get; set; } = new List<PrescriptionDetailsDto>();
    }
} 