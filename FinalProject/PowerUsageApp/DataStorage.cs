using Newtonsoft.Json;

namespace PowerUsageApp
{
    public class DataStorage
    {
        private readonly string filePath;
        
        public DataStorage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "File path cannot be empty.");
            }

            this.filePath = filePath;
        }   
        
        public void SaveData(List<EnergyData> records)
        {
            try
            {
                if (records == null || records.Count == 0)
                {
                    Console.WriteLine("⚠ Warning: No data to save.");
                    return;
                }


                
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(records, Newtonsoft.Json.Formatting.Indented);

                
                File.WriteAllText(filePath, json);

                Console.WriteLine("✅ Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving data: {ex.Message}");
            }
        }
        
        public List<EnergyData> LoadData()
        {
            if (!File.Exists(filePath))
            {
                return new List<EnergyData>(); 
            }

            string json = File.ReadAllText(filePath); 
            List<EnergyData> loadedRecords = JsonConvert.DeserializeObject<List<EnergyData>>(json) ?? new List<EnergyData>(); 


            return loadedRecords;
        }
        
    }
}