namespace ContainerLoadingManagement;

// Liquid container; hazardous ones have a lower fill limit.
public class LiquidContainer : Container, IHazardNotifier
{
    public bool IsHazardous { get; private set; }
    private static int liquidCounter = 0;

    public LiquidContainer(double maxPayload, bool isHazardous)
        : base(maxPayload, "L")
    {
        IsHazardous = isHazardous;
        FillLimitPercentage = isHazardous ? 0.50 : 0.90;
        SerialNumber = $"KON-L-{++liquidCounter}";
    }

    public void NotifyHazard(string message)
    {
        Console.WriteLine($"[Hazard Notification] {SerialNumber}: {message}");
    }
}