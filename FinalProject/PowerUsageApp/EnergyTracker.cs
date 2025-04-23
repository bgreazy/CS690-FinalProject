using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerUsageApp
{
    public class EnergyTracker
    {        
        private List<EnergyData> Records;
        
        public EnergyTracker()
        {
            Records = new List<EnergyData>();
        }
        
        public void AddEntry(EnergyData data, bool showMessage = true)
        {
            Records.Add(data);
            if (showMessage)
            {
                Console.WriteLine("Energy data entry successfully added.");
            }
        }
       
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


        public void DisplayInsights()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Insights:");
            Console.WriteLine("================================");
            
            Console.WriteLine($"Number of records: {Records.Count}");
            
            foreach (var record in Records)
            {
                double total = record.Usage * record.Cost;
                Console.WriteLine($"Date: {record.Date.ToShortDateString()}, Usage: {record.Usage} kWh, Cost: ${record.Cost:F2}, Total: ${total:F2}");
            }
        }

        
        public List<EnergyData> GetAllRecords()
        {
            return Records;
        }
    }
}