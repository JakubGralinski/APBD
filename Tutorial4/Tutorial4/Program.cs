namespace Tutorial3;

public class Program
{
    public static void Main(string[] args)
    {
        var emps = Database.GetEmps();

        decimal maxSalary = emps.Max(e => e.Sal);
        Console.WriteLine($"Maximum Salary: {maxSalary}");

        // Display employee-manager pairs using a self-join
        var employeeManagerPairs = emps
            .Where(e => e.Mgr.HasValue)
            .Join(emps,
                e => e.Mgr.Value,
                m => m.EmpNo,
                (e, m) => new { Employee = e.EName, Manager = m.EName })
            .ToList();

        Console.WriteLine("Employee-Manager Pairs:");
        foreach (var pair in employeeManagerPairs)
        {
            Console.WriteLine($"Employee: {pair.Employee}, Manager: {pair.Manager}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}