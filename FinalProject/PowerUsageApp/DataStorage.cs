using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PowerUsageApp
{
    public class DataStorage
    {
        private readonly string filePath;

        // Constructor to initialize file path
        public DataStorage(string filePath)
        {
            this.filePath = filePath;
        }

        // Method to save energy data to a file
        public void SaveData(List<EnergyData> records)
        {
            try
            {
                // Serialize the records to JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(records, Newtonsoft.Json.Formatting.Indented);

                // Write JSON to file
                File.WriteAllText(filePath, json);
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
        
public List<EnergyData> LoadData()
{
    if (!File.Exists(filePath))
    {
        Console.WriteLine("No existing data found. Starting fresh.");
        return new List<EnergyData>();
    }

    string json = File.ReadAllText(filePath);

    // Explicitly using Newtonsoft.Json to deserialize
    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<EnergyData>>(json) ?? new List<EnergyData>();
}
        // Method to load energy data from a file
        // public List<EnergyData> LoadData()
        // {
        //     try
        //     {
        //         // Check if the file exists
        //         if (!File.Exists(filePath))
        //         {
        //             Console.WriteLine("No existing data found. Starting fresh.");
        //             return new List<EnergyData>();
        //         }

        //         // Read JSON from file
        //         string json = File.ReadAllText(filePath);

        //         // Deserialize JSON back to a list of EnergyData
        //         return JsonSerializer.Deserialize<List<EnergyData>>(json) ?? new List<EnergyData>();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error loading data: {ex.Message}");
        //         return new List<EnergyData>();
        //     }
        // }
    }
}