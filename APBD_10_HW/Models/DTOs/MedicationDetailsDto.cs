namespace APBD_10_HW.Models.DTOs
{
    public class MedicationDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int? Dose { get; set; }
        public string? Details { get; set; }
    }
} 