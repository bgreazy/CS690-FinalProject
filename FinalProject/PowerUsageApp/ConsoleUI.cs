using System;

namespace PowerUsageApp
{
    public class Menu
    {
        private DataStorage storage;
        private EnergyTracker tracker;
        private GoalManager goalManager;
        private RecommendationEngine engine = new RecommendationEngine();
        



        public Menu(GoalManager goalManager)
        {
            this.goalManager = goalManager;

            string filePath = "energyData.json";  
            tracker = new EnergyTracker(filePath); 

            DataStorage storage = new DataStorage(filePath);
            
            
            List<EnergyData> loadedRecords = storage.LoadData();
            foreach (var record in loadedRecords)
            {
                tracker.GetAllRecords().Add(record); 
            }
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.WriteLine("================================");
                Console.WriteLine("Welcome to Power Usage App");
                Console.WriteLine("================================");
                Console.WriteLine("1. Add Energy Data");
                Console.WriteLine("2. View Insights");
                Console.WriteLine("3. Set Energy Goals");
                Console.WriteLine("4. View Recommendations");
                Console.WriteLine("5. View Energy Goals");
                Console.WriteLine("6. Delete Energy goal");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    HandleMenuSelection(choice);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
                }
            }
        }

        public void HandleMenuSelection(int choice)
        {
            switch (choice)
            {
                case 1:
                    AddEnergyData();
                    break;
                case 2:
                    ViewInsights();
                    break;
                case 3:
                    SetEnergyGoals();
                    break;
                case 4:
                    ViewRecommendations();
                    break;
                case 5:
                    ViewEnergyGoals();
                    break;
                case 6:
                    DeleteEnergyGoal();
                    break;
                case 7:
                    ExitApplication();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }

        private void AddEnergyData()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Add Energy Data");
            Console.WriteLine("================================");

            Console.Write("Enter the date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date (yyyy-MM-dd).");
                return;
            }

            Console.Write("Enter energy usage (kWh): ");
            if (!double.TryParse(Console.ReadLine(), out double usage) || usage <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive numeric value for usage.");
                return;
            }

            Console.Write("Enter cost per kWh ($): ");
            if (!double.TryParse(Console.ReadLine(), out double cost) || cost <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive numeric value.");
                return;
            }

            EnergyData data = new EnergyData(date, usage, cost);
            tracker.AddEntry(data);

            
            foreach (var goal in goalManager.Goals)
            {
                goal.TrackUsage(data.Usage, data.Date); 
            }

            goalManager.SaveGoals(); 
            storage.SaveData(tracker.GetAllRecords());

            Console.WriteLine("✅ Energy data entry successfully added.");
        }

        private void ViewInsights()
        {
            Console.WriteLine("================================");
            Console.WriteLine("View Insights");
            Console.WriteLine("================================");
            
            tracker.DisplayInsights(); 
            
        }

        private void SetEnergyGoals()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Set Energy Goals");
            Console.WriteLine("================================");

            
            Console.Write("Enter reduction goal (e.g., 10% or 100 kWh): ");
            if (!double.TryParse(Console.ReadLine(), out double goal) || goal <= 0)
            {
                Console.WriteLine("Error: Please enter a valid positive goal.");
                return;
            }

            Console.Write("Enter start date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Error: Invalid date format. Use yyyy-MM-dd.");
                return;
            }

            Console.Write("Enter end date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Error: Invalid date format. Use yyyy-MM-dd.");
                return;
            }

            if (startDate >= endDate)
            {
                Console.WriteLine("Error: Start date must be earlier than the end date.");
                return;
            }

            
            goalManager.SetGoal(goal, startDate, endDate);

            Console.WriteLine($"Energy goal set successfully!");
            Console.WriteLine($"Goal: Reduce energy by {goal}% between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}.");
        }

        private void ViewEnergyGoals()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Your Energy Goals Progress");
            Console.WriteLine("================================");

            
            if (goalManager == null || goalManager.Goals == null || goalManager.Goals.Count == 0)
            {
                Console.WriteLine("No active goals set. Please create an energy goal first.");
                return;
            }

            goalManager.DisplayGoals();

            
            if (tracker == null)
            {
                Console.WriteLine("⚠ Energy tracker not initialized. Unable to fetch records.");
                return;
            }

            List<EnergyData> allRecords = tracker.GetAllRecords();

            
            if (allRecords == null || allRecords.Count == 0)
            {
                Console.WriteLine("⚠ No energy records available to calculate progress.");
                return;
            }

            foreach (var goal in goalManager.Goals)
            {
                Console.WriteLine($"📅 Period: {goal.GoalStartDate.ToShortDateString()} → {goal.GoalEndDate.ToShortDateString()}");

                
                List<EnergyData> relevantRecords = allRecords.Where(r => r.Date < goal.GoalStartDate).ToList();
                double baselineUsage = relevantRecords.Count > 0 ? relevantRecords.Average(r => r.Usage) : 0;

                if (baselineUsage == 0)
                {
                    Console.WriteLine("⚠ No valid historical baseline found. Progress cannot be calculated.");
                    continue;
                }

                
                foreach (var record in allRecords)
                {
                    goal.TrackUsage(record.Usage, record.Date);
                }

                Console.WriteLine($"⚡ Energy Usage Tracked: {goal.TotalEnergyUsageDuringGoal} kWh");

                double progress = goal.CalculateProgress(baselineUsage);

                
                if (goal.TotalEnergyUsageDuringGoal > baselineUsage)
                {
                    Console.WriteLine("⚠ Energy usage has increased—goal not achieved!");
                }
                else
                {
                    Console.WriteLine($"Progress toward goal: {progress:F2}% achieved.");
                    Console.WriteLine(progress >= goal.ReductionGoal ? "🎉 Goal achieved!" : "⚡ Keep going! You’re making progress.");
                }
            }
        }
    

        private void ViewRecommendations()
        {
            Console.WriteLine("================================");
            Console.WriteLine("View Recommendations");
            Console.WriteLine("================================");
            Console.WriteLine("1. General Tips");
            Console.WriteLine("2. Custom Tips");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                engine.DisplayGeneralTips(); 
            }
            else if (choice == 2)
            {
        
                List<EnergyData> allRecords = tracker.GetAllRecords();

                if (allRecords.Count > 0)
                {
                    EnergyData recentData = allRecords.Last();                
                    engine.DisplayCustomTips(recentData);
                }
                else
                {
                    Console.WriteLine("No energy data found. Please add records to receive custom recommendations.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

            public void DeleteEnergyGoal()
            {
                Console.Write("🗑 Enter the end date of the goal to delete (MM/DD/YYYY): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime targetDate))
                {
                    goalManager.DeleteGoal(targetDate);
                }
                else
                {
                    Console.WriteLine("❌ Invalid date format. Please enter a valid date.");
                }
            }


            private void ExitApplication()
            {
                Console.WriteLine("Thank you for using the Power Usage Application. Goodbye!");
                Environment.Exit(0);
            }
    }
}
