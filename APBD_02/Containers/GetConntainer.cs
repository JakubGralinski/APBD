namespace ContainerLoadingManagement;

// Gas container; when emptying, leaves 5% of its cargo.
public class GasContainer : Container, IHazardNotifier
{
    public double Pressure { get; private set; } // in atmospheres
    private static int gasCounter = 0;

    public GasContainer(double maxPayload, double pressure)
        : base(maxPayload, "G")
    {
        Pressure = pressure;
        SerialNumber = $"KON-G-{++gasCounter}";
    }

    public override void EmptyCargo()
    {
        double remaining = CurrentCargo * 0.05;
        Console.WriteLine($"Emptying gas container {SerialNumber}. Leaving {remaining} kg (5%) of cargo.");
        CurrentCargo = remaining;
    }

    public void NotifyHazard(string message)
    {
        Console.WriteLine($"[Hazard Notification] {SerialNumber}: {message}");
    }
}