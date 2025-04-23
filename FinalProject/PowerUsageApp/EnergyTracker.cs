using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace PowerUsageApp
{
    public class EnergyTracker
    {        
        private List<EnergyData> Records;
        private DataStorage storage;
        private string filePath; 

        public EnergyTracker(string filePath)
        {
            this.filePath = filePath; 
            storage = new DataStorage(filePath);
            
            
            Records = LoadData();
        }

        public void AddEntry(EnergyData data, bool showMessage = true)
        {
            Records.Add(data);
            
            if (showMessage)
            {
                Console.WriteLine("✅ Energy data entry successfully added."); 
            }

            SaveEnergyData(); 
        }

        public void SaveEnergyData()
        {
            var stackTrace = Environment.StackTrace;
            Console.WriteLine("🔍 DEBUG: SaveEnergyData() called from:");
            Console.WriteLine(stackTrace);

            storage.SaveData(Records);
            Console.WriteLine("✅ Energy data saved successfully.");
        }

        public List<EnergyData> LoadData()
        {
            Console.WriteLine("🔍 DEBUG: LoadData() called");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("ℹ No saved energy data found.");
                return new List<EnergyData>(); 
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var records = JsonSerializer.Deserialize<List<EnergyData>>(json) ?? new List<EnergyData>();

                Console.WriteLine("✅ Energy data loaded successfully.");
                return records;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading energy data: {ex.Message}");
                return new List<EnergyData>();
            }
        }

        public double CalculateTotalUsageForDay(DateTime day)
        {
            var recordsForDay = Records.Where(record => record.Date.Date == day.Date).ToList();
            
            if (recordsForDay.Count == 0)
            {
                Console.WriteLine($"⚠ No records found for {day.ToShortDateString()}.");
                return 0;
            }

            double totalUsage = recordsForDay.Sum(record => record.Usage);
            Console.WriteLine($"🔍 Total usage for {day.ToShortDateString()}: {totalUsage} kWh");
            return totalUsage;
        }

        public void DisplayInsights()
        {
            Console.WriteLine("================================");
            Console.WriteLine("📊 Insights:");
            Console.WriteLine("================================");

            Console.WriteLine($"📌 Number of records: {Records.Count}");

            foreach (var record in Records)
            {
                double total = record.Usage * record.Cost;
                Console.WriteLine($"📅 Date: {record.Date.ToShortDateString()}, ⚡ Usage: {record.Usage} kWh, 💲 Cost: ${record.Cost:F2}, 📈 Total: ${total:F2}");
            }
        }

        public List<EnergyData> GetAllRecords()
        {
            return Records;
        }
    }
}