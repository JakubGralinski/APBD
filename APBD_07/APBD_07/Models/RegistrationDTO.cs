namespace APBD_07.Models;

public class RegistrationDto
{
    public int     TripId        { get; set; }
    public string  TripName      { get; set; }
    public DateTime RegisteredAt { get; set; }
    public decimal? PaymentAmount{ get; set; }
}