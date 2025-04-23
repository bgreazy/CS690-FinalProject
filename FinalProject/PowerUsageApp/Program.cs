namespace PowerUsageApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            GoalManager goalManager = new GoalManager("goals.json"); 
            List<EnergyGoal> activeGoals = goalManager.Goals;

            Menu menu = new Menu(goalManager);
            
            while (true) 
            {
                menu.DisplayMenu();
                
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 7) 
                    {
                        Console.WriteLine("Thank you for using the Power Usage Application. Goodbye!");
                        break;
                    }

                    menu.HandleMenuSelection(choice);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }
    }
}
