using System;

class Calculator
{
    public static double FindAverage(int[] numbers)
    {
        if (numbers == null || numbers.Length == 0)
        {
            throw new ArgumentException("Array cannot be null or empty");
        }

        int sum = 0;
        foreach (int number in numbers)  // Unified resolution: using 'number'
        {
            sum += number;
        }
        return (double)sum / numbers.Length;
    }
    
    public static int FindMax(int[] numbers)
    {
        if (numbers == null || numbers.Length == 0)
        {
            throw new ArgumentException("Array cannot be null or empty");
        }

        int max = numbers[0];
        foreach (int num in numbers)
        {
            if (num > max)
            {
                max = num;
            }
        }
        return max;
    }
    
    static void Main()
    {
        int[] arr1 = { 1, 2, 3, 4, 5 };
        Console.WriteLine("Average: " + FindAverage(arr1));
        
        int[] arr2 = { 1, 3, 7, 0, 5 };
        Console.WriteLine("Max value: " + FindMax(arr2));
    }
}
