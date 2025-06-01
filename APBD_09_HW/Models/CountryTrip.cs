namespace APBD_09_HW.Models
{
    public class CountryTrip
    {
        public int IdCountry { get; set; }
        public int IdTrip { get; set; }

        public Country Country { get; set; } = null!;
        public Trip Trip { get; set; } = null!;
    }
} 