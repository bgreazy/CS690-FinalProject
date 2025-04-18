using System;

namespace PowerUsageApp
{
    public class EnergyData
    {
        // Attributes
        public DateTime Date { get; set; }   // Date of the energy usage entry
        public double Usage { get; set; }   // Energy usage in kWh
        public double Cost { get; set; }    // Cost of the energy usage in dollars

        // Constructor
        public EnergyData(DateTime date, double usage, double cost)
        {
            Date = date;
            Usage = usage;
            Cost = cost;
        }

        // Method to validate the data
        public bool ValidateData()
        {
            // Ensure that usage and cost are non-negative
            if (Usage < 0 || Cost < 0)
            {
                Console.WriteLine("Error: Usage and cost values must be non-negative.");
                return false;
            }

            // Additional validation logic can be added here if needed
            return true;
        }

        // Method to display the energy data as a string
        public override string ToString()
        {
            return $"Date: {Date.ToShortDateString()}, Usage: {Usage} kWh, Cost: ${Cost}";
        }
    }
}