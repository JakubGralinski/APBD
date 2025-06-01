using System.Collections.Generic;

namespace APBD_09_HW.Models
{
    public class Country
    {
        public int IdCountry { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<CountryTrip> CountryTrips { get; set; } = new List<CountryTrip>();
    }
} 