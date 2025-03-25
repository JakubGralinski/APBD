using System;
using System.Collections.Generic;
using System.IO;

namespace ContainerLoadingManagement
{
    // Exception thrown when trying to overfill a container.
    public class OverfillException : Exception
    {
        public OverfillException(string message) : base(message) { }
    }

    // Main Program demonstrating usage.
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Create example containers:
                // Liquid container with hazardous cargo.
                LiquidContainer liquidCont = new LiquidContainer(10000, true); // max payload 10,000 kg, hazardous => fill limit 50%
                // Gas container with pressure info.
                GasContainer gasCont = new GasContainer(8000, 2.5);  // max payload 8,000 kg, pressure 2.5 atm
                // Refrigerated container for Milk.
                RefrigeratedContainer refrigCont = new RefrigeratedContainer(12000, 250, 3000, 150, "Milk", 4);

                // Load cargo into containers:
                try
                {
                    liquidCont.LoadCargo(4000); // 50% of 10000 = 5000, so 4000 is OK.
                    gasCont.LoadCargo(7000);    // 90% of 8000 = 7200, so 7000 is OK.
                    refrigCont.LoadCargo(10000); // 90% of 12000 = 10800, so 10000 is OK.
                }
                catch (OverfillException ex)
                {
                    Console.WriteLine("Error loading cargo: " + ex.Message);
                }

                // Create a container ship.
                ContainerShip ship = new ContainerShip("Ship 1", 10, 100, 40000); // 40,000 tons capacity
                // Load containers onto the ship.
                ship.LoadContainer(liquidCont);
                ship.LoadContainer(gasCont);
                ship.LoadContainer(refrigCont);

                // Print ship information.
                ship.PrintShipInfo();

                // Unload a container.
                ship.UnloadContainer(liquidCont.SerialNumber);
                ship.PrintShipInfo();

                // Optionally: demonstrate hazard notification.
                liquidCont.NotifyHazard("Hazardous liquid detected!");
                gasCont.NotifyHazard("Gas leak detected!");
                refrigCont.NotifyHazard("Temperature out of range!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}