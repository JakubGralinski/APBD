namespace APBD_07.Models;

public class TripDto
{
    public int      Id              { get; set; }
    public string   Name            { get; set; }
    public string   Description     { get; set; }
    public DateTime StartDate       { get; set; }
    public DateTime EndDate         { get; set; }
    public int      MaxParticipants { get; set; }
    public string   Country         { get; set; }
}