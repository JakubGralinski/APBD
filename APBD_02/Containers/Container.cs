namespace ContainerLoadingManagement;

// Abstract base class for all containers.
public abstract class Container
{
    public double MaxPayload { get; protected set; }     // Maximum cargo capacity (kg)
    public double CurrentCargo { get; protected set; }     // Current loaded cargo (kg)
    public string ContainerType { get; protected set; }    // E.g., "L", "G", "C"
    public double FillLimitPercentage { get; protected set; }  // Allowed fill percentage (0.50 or 0.90)
    public string SerialNumber { get; protected set; }     // Unique serial number

    protected Container(double maxPayload, string containerType)
    {
        MaxPayload = maxPayload;
        CurrentCargo = 0;
        ContainerType = containerType;
        FillLimitPercentage = 0.90;  // default fill limit is 90%
    }

    // Loads cargo into the container.
    public virtual void LoadCargo(double mass)
    {
        double limit = MaxPayload * FillLimitPercentage;
        if (CurrentCargo + mass > limit)
            throw new OverfillException($"Loading {mass} kg exceeds allowed limit of {limit} kg.");
        CurrentCargo += mass;
        Console.WriteLine($"Loaded {mass} kg into container {SerialNumber}. Current cargo: {CurrentCargo} kg.");
    }

    // Empties the container.
    public virtual void EmptyCargo()
    {
        Console.WriteLine($"Emptying container {SerialNumber}. Previous cargo: {CurrentCargo} kg.");
        CurrentCargo = 0;
    }

    public override string ToString()
    {
        return $"Container {SerialNumber} [{ContainerType}] - MaxPayload: {MaxPayload} kg, Cargo: {CurrentCargo} kg";
    }
}