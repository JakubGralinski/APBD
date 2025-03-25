namespace ContainerLoadingManagement;

 // ContainerShip class to manage containers.
    public class ContainerShip
    {
        public string Name { get; private set; }
        public double MaxSpeed { get; private set; }         // in knots
        public int MaxContainerCount { get; private set; }
        public double MaxTotalWeight { get; private set; }     // in tons
        public List<Container> Containers { get; private set; }

        public ContainerShip(string name, double maxSpeed, int maxContainerCount, double maxTotalWeight)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            MaxContainerCount = maxContainerCount;
            MaxTotalWeight = maxTotalWeight;
            Containers = new List<Container>();
        }

        public void LoadContainer(Container container)
        {
            if (Containers.Count >= MaxContainerCount)
            {
                Console.WriteLine("Cannot load container: maximum container count reached.");
                return;
            }
            double currentWeight = 0;
            foreach (var c in Containers)
            {
                currentWeight += c.CurrentCargo;
            }
            // Convert tons to kg: 1 ton = 1000 kg
            if (currentWeight + container.CurrentCargo > MaxTotalWeight * 1000)
            {
                Console.WriteLine("Cannot load container: maximum total weight exceeded.");
                return;
            }
            Containers.Add(container);
            Console.WriteLine($"Container {container.SerialNumber} loaded onto ship {Name}.");
        }

        public void UnloadContainer(string serialNumber)
        {
            Container found = Containers.Find(c => c.SerialNumber == serialNumber);
            if (found != null)
            {
                Containers.Remove(found);
                Console.WriteLine($"Container {serialNumber} unloaded from ship {Name}.");
            }
            else
            {
                Console.WriteLine($"Container {serialNumber} not found on ship {Name}.");
            }
        }

        public void PrintShipInfo()
        {
            Console.WriteLine($"Ship: {Name}");
            Console.WriteLine($"Max Speed: {MaxSpeed} knots");
            Console.WriteLine($"Max Containers: {MaxContainerCount}");
            Console.WriteLine($"Max Total Weight: {MaxTotalWeight} tons");
            Console.WriteLine($"Loaded Containers: {Containers.Count}");
            foreach (var container in Containers)
            {
                Console.WriteLine(container);
            }
        }
    }