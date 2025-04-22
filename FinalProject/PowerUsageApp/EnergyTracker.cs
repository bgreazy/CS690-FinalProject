using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerUsageApp
{
    public class EnergyTracker
    {
        // List to store all energy records
        private List<EnergyData> Records;

        // Constructor to initialize the list
        public EnergyTracker()
        {
            Records = new List<EnergyData>();
        }

        // Method to add a new energy record
        public void AddEntry(EnergyData data)
        {
            Records.Add(data);
            Console.WriteLine("Energy data entry successfully added.");
        }

        // Method to calculate total usage for a specific day
        public double CalculateTotalUsageForDay(DateTime day)
        {
            var recordsForDay = Records.Where(record => record.Date.Date == day.Date).ToList();
            if (recordsForDay.Count == 0)
            {
                Console.WriteLine($"No records found for the date {day.ToShortDateString()}.");
                return 0;
            }

            double totalUsage = recordsForDay.Sum(record => record.Usage);
            Console.WriteLine($"Total usage for {day.ToShortDateString()}: {totalUsage} kWh");
            return totalUsage;
        }

        // Method to display insights
        public void DisplayInsights()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Insights:");
            Console.WriteLine("================================");

            // Display the total number of records
            Console.WriteLine($"Number of records: {Records.Count}");

            // Display usage, cost, and total for each record
            // Console.WriteLine("Usage, Cost, and Total Cost Per Record:");
            foreach (var record in Records)
            {
                double total = record.Usage * record.Cost; // Calculate total for the record
                Console.WriteLine($"Date: {record.Date.ToShortDateString()}, Usage: {record.Usage} kWh, Cost: ${record.Cost:F2}, Total: ${total:F2}");
            }
        }

        // Method to retrieve all records
        public List<EnergyData> GetAllRecords()
        {
            return Records;
        }
    }
}