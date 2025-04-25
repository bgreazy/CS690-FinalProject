using System.Text.Json;

namespace PowerUsageApp
{
    public class GoalManager
    {  
        public string FilePath { get; private set; } 
        public List<EnergyGoal> Goals { get; private set; } = new List<EnergyGoal>();

        public GoalManager(string filePath)
        {
            this.FilePath = filePath;
            Goals = LoadGoals();
        }

        public List<EnergyGoal> LoadGoals()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("ℹ No saved goals found.");
                return new List<EnergyGoal>();
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                var goals = JsonSerializer.Deserialize<List<EnergyGoal>>(json, new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                }) ?? new List<EnergyGoal>();


                Console.WriteLine("✅ Goals loaded successfully.");
                return goals;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading goals: {ex.Message}");
                return new List<EnergyGoal>();
            }
        }

        public void SaveGoals()
        {
            if (Goals == null) return;

            string json = JsonSerializer.Serialize(Goals, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
            //Console.WriteLine("✅ Goals saved successfully.");
        }

        public void SetGoal(double reductionGoal, DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                Console.WriteLine("❌ Error: Start date must be earlier than the end date.");
                return;
            }

            EnergyGoal newGoal = new EnergyGoal(reductionGoal, startDate, endDate, 0);
            Goals.Add(newGoal);

            SaveGoals();
            
        }

        public bool DeleteGoal(DateTime targetDate)
        {
            var goalToRemove = Goals.FirstOrDefault(g => g.GoalEndDate.Date == targetDate.Date);

            if (goalToRemove != null)
            {
                Goals.Remove(goalToRemove);
                SaveGoals(); 
                Console.WriteLine($"🗑 Goal for {targetDate:yyyy-MM-dd} removed successfully.");
                return true; // ✅ Return success
            }
            
            Console.WriteLine($"❌ No goal found for {targetDate:yyyy-MM-dd}.");
            return false; // ✅ Return failure
        }

        public void DisplayGoals()
        {

            Goals = LoadGoals();

            if (Goals == null || Goals.Count == 0)
            {
                Console.WriteLine("No active goals set. Please create an energy goal first.");
                return;
            }

        }
    }
}