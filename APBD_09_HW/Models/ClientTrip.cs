using System;

namespace APBD_09_HW.Models
{
    public class ClientTrip
    {
        public int IdClient { get; set; }
        public int IdTrip { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? PaymentDate { get; set; }

        public Client Client { get; set; } = null!;
        public Trip Trip { get; set; } = null!;
    }
} 