using System;

namespace PowerUsageApp
{
    public class Menu
    {
        private EnergyTracker tracker = new EnergyTracker();
        private GoalManager goalManager = new GoalManager();
        private RecommendationEngine engine = new RecommendationEngine();

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
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    HandleMenuSelection(choice);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
                }
            }
        }

        private void HandleMenuSelection(int choice)
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
            DateTime date = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter energy usage (kWh): ");
            double usage = double.Parse(Console.ReadLine());
            Console.Write("Enter cost ($): ");
            double cost = double.Parse(Console.ReadLine());

            EnergyData data = new EnergyData(date, usage, cost);
            if (data.ValidateData())
            {
                tracker.AddEntry(data);
                Console.WriteLine("Energy data added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid data entered. Please try again.");
            }
        }

        private void ViewInsights()
        {
            Console.WriteLine("================================");
            Console.WriteLine("View Insights");
            Console.WriteLine("================================");
            // Implement logic to retrieve and display insights using EnergyTracker
            tracker.DisplayInsights(); // Placeholder method
        }

        private void SetEnergyGoals()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Set Energy Goals");
            Console.WriteLine("================================");
            Console.Write("Enter reduction goal (e.g., 10% or 100 kWh): ");
            double goal = double.Parse(Console.ReadLine());
            Console.Write("Enter start date (yyyy-MM-dd): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter end date (yyyy-MM-dd): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            goalManager.SetGoal(goal, startDate, endDate);
            Console.WriteLine("Energy goal set successfully!");
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
                engine.DisplayGeneralTips(); // Placeholder method
            }
            else if (choice == 2)
            {
                EnergyData recentData = new EnergyData(DateTime.Now, 30.0, 100.0); // Replace with real data
                engine.DisplayCustomTips(recentData);
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        private void ExitApplication()
        {
            Console.WriteLine("Thank you for using the Power Usage Application. Goodbye!");
            Environment.Exit(0);
        }
    }
}
