using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PowerUsageApp
{
    public class DataStorage
    {
        private readonly string filePath;

        
        public DataStorage(string filePath)
        {
            this.filePath = filePath;
        }

        
        public void SaveData(List<EnergyData> records)
        {
            try
            {
                
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(records, Newtonsoft.Json.Formatting.Indented);

                
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

    
    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<EnergyData>>(json) ?? new List<EnergyData>();
}
        
    }
}