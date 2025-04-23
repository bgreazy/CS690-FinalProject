using System;
using System.Collections.Generic;
using System.IO;
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

                // foreach (var goal in goals)
                // {
                //     goal.LoadTrackedProgress(); 
                // }

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
            Console.WriteLine("✅ Goals saved successfully.");
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
            Console.WriteLine($"🎯 Goal set: Reduce energy by {newGoal.ReductionGoal}% from {newGoal.GoalStartDate.ToShortDateString()} to {newGoal.GoalEndDate.ToShortDateString()}.");
        }

        public void DeleteGoal(DateTime targetDate)
        {
            var goalToRemove = Goals.FirstOrDefault(g => g.GoalEndDate.Date == targetDate.Date);

            if (goalToRemove != null)
            {
                Goals.Remove(goalToRemove);
                SaveGoals(); 
                Console.WriteLine($"🗑 Goal for {targetDate.ToShortDateString()} removed successfully.");
            }
            else
            {
                Console.WriteLine($"❌ No goal found for {targetDate.ToShortDateString()}.");
            }
        }

        public void DisplayGoals()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Your Active Energy Goals");
            Console.WriteLine("================================");

            Goals = LoadGoals();

            if (Goals == null || Goals.Count == 0)
            {
                Console.WriteLine("No active goals set. Please create an energy goal first.");
                return;
            }

            foreach (var goal in Goals)
            {
                Console.WriteLine($"🎯 Goal: Reduce energy by {goal.ReductionGoal}%");
                //Console.WriteLine($"📅 Period: {goal.GoalStartDate.ToShortDateString()} → {goal.GoalEndDate.ToShortDateString()}");

                double totalTrackedUsage = goal.TotalEnergyUsageDuringGoal;
                //Console.WriteLine($"⚡ Energy Usage Tracked: {totalTrackedUsage} kWh");

                Console.WriteLine("--------------------------------");
            }
        }
    }
}