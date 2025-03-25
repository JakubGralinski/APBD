namespace ContainerLoadingManagement;

public class RefrigeratedContainer : Container, IHazardNotifier
{
    public double Height { get; private set; }       // in cm
    public double TareWeight { get; private set; }     // container's own weight (kg)
    public double Depth { get; private set; }          // in cm
    public string ProductType { get; private set; }    // Type of product stored
    public double Temperature { get; private set; }    // Temperature in the container
    private static int refrigeratedCounter = 0;

    public RefrigeratedContainer(double maxPayload, double height, double tareWeight, double depth, string productType, double temperature)
        : base(maxPayload, "C")
    {
        Height = height;
        TareWeight = tareWeight;
        Depth = depth;
        ProductType = productType;
        Temperature = temperature;
        SerialNumber = $"KON-C-{++refrigeratedCounter}";
    }

    // You can override LoadCargo to enforce product type consistency or temperature rules.
    public void NotifyHazard(string message)
    {
        Console.WriteLine($"[Hazard Notification] {SerialNumber}: {message}");
    }
}