using System;
using Spectre.Console;

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
            this.goalManager = goalManager ?? throw new ArgumentNullException(nameof(goalManager)); 

            string filePath = "energyData.json";  
            tracker = new EnergyTracker(filePath) ?? throw new ArgumentNullException(nameof(tracker)); 

            storage = new DataStorage(filePath) ?? throw new ArgumentNullException(nameof(storage)); 

            
        }

        

        public void DisplayMenu()
        {
            AnsiConsole.Markup("[bold cyan]Welcome to Power Usage App[/]");
            AnsiConsole.WriteLine();
            var menu = new SelectionPrompt<string>()
                .Title("[bold]Choose an option:[/]")
                .AddChoices("1. Add Energy Data", "2. View Insights", "3. Set Energy Goals", "4. View Recommendations",
                 "5. View energy Goals", "6. Delete Energy Goals", "7. Delete Energy Data", "8. Exit");
            
            string choice = AnsiConsole.Prompt(menu);
            HandleMenuSelection(int.Parse(choice[0].ToString()));
        }

        // public void DisplayMenu()
        // {
        //     while (true)
        //     {
        //         Console.WriteLine("================================");
        //         Console.WriteLine("Welcome to Power Usage App");
        //         Console.WriteLine("================================");
        //         Console.WriteLine("1. Add Energy Data");
        //         Console.WriteLine("2. View Insights");
        //         Console.WriteLine("3. Set Energy Goals");
        //         Console.WriteLine("4. View Recommendations");
        //         Console.WriteLine("5. View Energy Goals");
        //         Console.WriteLine("6. Delete Energy goal");
        //         Console.WriteLine("7. Exit");
        //         Console.Write("Enter your choice: ");

        //         string? input = Console.ReadLine();
        //         if (string.IsNullOrWhiteSpace(input)) 
        //         {
        //             Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
        //             return;
        //         }

        //         if (int.TryParse(input, out int choice))
        //         {
        //             HandleMenuSelection(choice);
        //         }
        //         else
        //         {
        //             Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
        //         }
        //     }
        // }

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
                    tracker.DeleteEnergyData();
                    break;
                case 8:
                    ExitApplication();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
            DisplayMenu();
        }

        // private void AddEnergyData()
        // {
        //     Console.WriteLine("================================");
        //     Console.WriteLine("Add Energy Data");
        //     Console.WriteLine("================================");

        //     if (tracker == null)
        //     {
        //         Console.WriteLine("❌ Error: Energy tracker not initialized.");
        //         return;
        //     }

        //     Console.Write("Enter the date (yyyy-MM-dd): ");
        //     if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
        //     {
        //         Console.WriteLine("❌ Invalid date format.");
        //         return;
        //     }

        //     Console.Write("Enter energy usage (kWh): ");
        //     if (!double.TryParse(Console.ReadLine(), out double usage) || usage <= 0)
        //     {
        //         Console.WriteLine("❌ Invalid input.");
        //         return;
        //     }

        //     Console.Write("Enter cost per kWh ($): ");
        //     if (!double.TryParse(Console.ReadLine(), out double cost) || cost <= 0)
        //     {
        //         Console.WriteLine("❌ Invalid input.");
        //         return;
        //     }

        //     EnergyData data = new EnergyData(date, usage, cost);

        //     tracker.AddEntry(data); 

        //     tracker.SaveEnergyData(); 
        // }
        

        private void AddEnergyData()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[bold cyan]⚡ Add Energy Data[/]\n");
            AnsiConsole.WriteLine("==============================");

            if (tracker == null)
            {
                AnsiConsole.Markup("[red]❌ Error: Energy tracker not initialized.[/]");
                return;
            }

            // 📅 Date Input with Validation
            var date = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter the date after the year 2000 (yyyy-MM-dd):")
                    .Validate(input => input.Year > 2000 ? ValidationResult.Success() : ValidationResult.Error("[red]❌ Invalid date format.[/]"))
            );

            // ⚡ Energy Usage Input
            var usage = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter energy usage (kWh):")
                    .Validate(input => input > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]❌ Invalid input.[/]"))
            );

            // 💲 Cost Input
            var cost = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter cost per kWh ($):")
                    .Validate(input => input > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]❌ Invalid input.[/]"))
            );

            EnergyData data = new EnergyData(date, usage, cost);
            tracker.AddEntry(data);

            AnsiConsole.Markup("[green]✅ Energy data entry successfully added.[/]\n");

            tracker.SaveEnergyData();
            AnsiConsole.Markup("[green]✅ Energy data saved successfully.[/]\n");
        }

        private void ViewInsights()
        {
            // Console.WriteLine("================================");
            // Console.WriteLine("View Insights");
            // Console.WriteLine("================================");
            
            tracker.DisplayInsights(); 
            
        }

        // private void SetEnergyGoals()
        // {
        //     Console.WriteLine("================================");
        //     Console.WriteLine("Set Energy Goals");
        //     Console.WriteLine("================================");

            
        //     Console.Write("Enter reduction goal (e.g., 10% or 100 kWh): ");
        //     if (!double.TryParse(Console.ReadLine(), out double goal) || goal <= 0)
        //     {
        //         Console.WriteLine("Error: Please enter a valid positive goal.");
        //         return;
        //     }

        //     Console.Write("Enter start date (yyyy-MM-dd): ");
        //     if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        //     {
        //         Console.WriteLine("Error: Invalid date format. Use yyyy-MM-dd.");
        //         return;
        //     }

        //     Console.Write("Enter end date (yyyy-MM-dd): ");
        //     if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
        //     {
        //         Console.WriteLine("Error: Invalid date format. Use yyyy-MM-dd.");
        //         return;
        //     }

        //     if (startDate >= endDate)
        //     {
        //         Console.WriteLine("Error: Start date must be earlier than the end date.");
        //         return;
        //     }

            
        //     goalManager.SetGoal(goal, startDate, endDate);

        //     Console.WriteLine($"Energy goal set successfully!");
        //     Console.WriteLine($"Goal: Reduce energy by {goal}% between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}.");
        // }

        

        private void SetEnergyGoals()
        {
            AnsiConsole.Markup("[bold cyan]🏆 Set Energy Goals[/]\n");
            AnsiConsole.WriteLine("==============================");

            // ✅ Reduction Goal Input
            var goal = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter reduction goal (e.g., 10% or 100 kWh):")
                    .Validate(input => input > 0 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]❌ Error: Please enter a valid positive goal.[/]"))
            );

            // 📅 Start Date Input
            var startDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter start date (yyyy-MM-dd):")
                    .Validate(input => input.Year > 2000 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]❌ Error: Invalid date format. Use yyyy-MM-dd.[/]"))
            );

            // 📅 End Date Input
            var endDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter end date (yyyy-MM-dd):")
                    .Validate(input => input > startDate 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]❌ Error: Start date must be earlier than the end date.[/]"))
            );

            goalManager.SetGoal(goal, startDate, endDate);

            AnsiConsole.Markup($"\n[green]✅ Energy goal set successfully![/]\n");
            AnsiConsole.WriteLine("==============================");
            AnsiConsole.Markup($"[bold]Goal:[/] Reduce energy by [bold yellow]{goal}[/] between [bold blue]{startDate:yyyy-MM-dd}[/] and [bold blue]{endDate:yyyy-MM-dd}[/].\n\n");
        }

        // private void ViewEnergyGoals()
        // {
        //     Console.WriteLine("================================");
        //     Console.WriteLine("Your Energy Goals Progress");
        //     Console.WriteLine("================================");

            
        //     if (goalManager == null || goalManager.Goals == null || goalManager.Goals.Count == 0)
        //     {
        //         Console.WriteLine("No active goals set. Please create an energy goal first.");
        //         return;
        //     }

        //     goalManager.DisplayGoals();

            
        //     if (tracker == null)
        //     {
        //         Console.WriteLine("⚠ Energy tracker not initialized. Unable to fetch records.");
        //         return;
        //     }

        //     List<EnergyData> allRecords = tracker.GetAllRecords();

            
        //     if (allRecords == null || allRecords.Count == 0)
        //     {
        //         Console.WriteLine("⚠ No energy records available to calculate progress.");
        //         return;
        //     }

        //     foreach (var goal in goalManager.Goals)
        //     {
        //         Console.WriteLine($"📅 Period: {goal.GoalStartDate.ToShortDateString()} → {goal.GoalEndDate.ToShortDateString()}");

                
        //         List<EnergyData> relevantRecords = allRecords.Where(r => r.Date < goal.GoalStartDate).ToList();
        //         double baselineUsage = relevantRecords.Count > 0 ? relevantRecords.Average(r => r.Usage) : 0;

        //         if (baselineUsage == 0)
        //         {
        //             Console.WriteLine("⚠ No valid historical baseline found. Progress cannot be calculated.");
        //             continue;
        //         }

                
        //         foreach (var record in allRecords)
        //         {
        //             goal.TrackUsage(record.Usage, record.Date);
        //         }

        //         Console.WriteLine($"⚡ Energy Usage Tracked: {goal.TotalEnergyUsageDuringGoal} kWh");

        //         double progress = goal.CalculateProgress(baselineUsage);

                
        //         if (goal.TotalEnergyUsageDuringGoal > baselineUsage)
        //         {
        //             Console.WriteLine("⚠ Energy usage has increased—goal not achieved!");
        //         }
        //         else
        //         {
        //             Console.WriteLine($"Progress toward goal: {progress:F2}% achieved.");
        //             Console.WriteLine(progress >= goal.ReductionGoal ? "🎉 Goal achieved!" : "⚡ Keep going! You’re making progress.");
        //         }
        //     }
        // }

        

        private void ViewEnergyGoals()
        {
            AnsiConsole.Markup("[bold cyan]🏆 Your Energy Goals Progress[/]\n");
            AnsiConsole.WriteLine("==============================");

            // ✅ Check if there are active goals
            if (goalManager == null || goalManager.Goals == null || goalManager.Goals.Count == 0)
            {
                AnsiConsole.Markup("[yellow]⚠ No active goals set. Please create an energy goal first.[/]\n");
                return;
            }

            goalManager.DisplayGoals();

            // ✅ Ensure energy tracker is initialized
            if (tracker == null)
            {
                AnsiConsole.Markup("[red]⚠ Error: Energy tracker not initialized. Unable to fetch records.[/]\n");
                return;
            }

            List<EnergyData> allRecords = tracker.GetAllRecords();

            // ✅ Verify records are available for progress calculation
            if (allRecords == null || allRecords.Count == 0)
            {
                AnsiConsole.Markup("[yellow]⚠ No energy records available to calculate progress.[/]\n");
                return;
            }

            var table = new Table();
            table.AddColumn("📅 Period");
            table.AddColumn("📉 Baseline Usage (kWh)");
            table.AddColumn("⚡ Tracked Usage (kWh)");
            table.AddColumn("📈 Progress");
            table.AddColumn("🎯 Status");

            foreach (var goal in goalManager.Goals)
            {
                List<EnergyData> relevantRecords = allRecords.Where(r => r.Date < goal.GoalStartDate).ToList();
                double baselineUsage = relevantRecords.Count > 0 ? relevantRecords.Average(r => r.Usage) : 0;

                if (baselineUsage == 0)
                {
                    table.AddRow($"{goal.GoalStartDate:yyyy-MM-dd} → {goal.GoalEndDate:yyyy-MM-dd}",
                                "N/A",
                                "N/A",
                                "[yellow]⚠ No baseline data[/]",
                                "[yellow]⚠ Cannot calculate progress[/]");
                    continue;
                }

                foreach (var record in allRecords)
                {
                    goal.TrackUsage(record.Usage, record.Date);
                }

                double progress = goal.CalculateProgress(baselineUsage);
                string status = progress >= goal.ReductionGoal ? "[green]🎉 Goal achieved![/]" : "[yellow]⚡ Keep going![/]";

                table.AddRow($"{goal.GoalStartDate:yyyy-MM-dd} → {goal.GoalEndDate:yyyy-MM-dd}",
                            $"{baselineUsage:F2} kWh",
                            $"{goal.TotalEnergyUsageDuringGoal:F2} kWh",
                            $"{progress:F2}%",
                            status);
            }

            AnsiConsole.Write(table);
        }
    

        // private void ViewRecommendations()
        // {
        //     Console.WriteLine("================================");
        //     Console.WriteLine("View Recommendations");
        //     Console.WriteLine("================================");
        //     Console.WriteLine("1. General Tips");
        //     Console.WriteLine("2. Custom Tips");
        //     Console.Write("Enter your choice: ");

        //     string? input = Console.ReadLine();
        //     if (string.IsNullOrWhiteSpace(input))
        //     {
        //         Console.WriteLine("❌ Invalid input. Please enter a number.");
        //         return;
        //     }

        //     if (!int.TryParse(input, out int choice))
        //     {
        //         Console.WriteLine("❌ Invalid number format. Please enter a valid number.");
        //         return;
        //     }

            
        //     HandleMenuSelection(choice);

        //     if (choice == 1)
        //     {
        //         engine.DisplayGeneralTips(); 
        //     }
        //     else if (choice == 2)
        //     {
        
        //         List<EnergyData> allRecords = tracker.GetAllRecords();

        //         if (allRecords.Count > 0)
        //         {
        //             EnergyData recentData = allRecords.Last();                
        //             engine.DisplayCustomTips(recentData);
        //         }
        //         else
        //         {
        //             Console.WriteLine("No energy data found. Please add records to receive custom recommendations.");
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("Invalid choice. Please try again.");
        //     }
        // }



        private void ViewRecommendations()
        {
            AnsiConsole.Markup("[bold cyan]🔍 View Recommendations[/]\n");
            AnsiConsole.WriteLine("==============================");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Choose a recommendation type:[/]")
                    .AddChoices("General Tips", "Custom Tips", "Back to Menu")
            );

            if (choice == "General Tips")
            {
                engine.DisplayGeneralTips();
            }
            else if (choice == "Custom Tips")
            {
                List<EnergyData> allRecords = tracker.GetAllRecords();

                if (allRecords.Count > 0)
                {
                    EnergyData recentData = allRecords.Last();
                    engine.DisplayCustomTips(recentData);
                }
                else
                {
                    AnsiConsole.Markup("[yellow]⚠ No energy data found. Please add records to receive custom recommendations.[/]\n");
                }
            }

            // ✅ Automatically return to main menu after showing recommendations
            DisplayMenu();
        }


            // public void DeleteEnergyGoal()
            // {
            //     Console.Write("🗑 Enter the end date of the goal to delete (MM/DD/YYYY): ");
            //     if (DateTime.TryParse(Console.ReadLine(), out DateTime targetDate))
            //     {
            //         goalManager.DeleteGoal(targetDate);
            //     }
            //     else
            //     {
            //         Console.WriteLine("❌ Invalid date format. Please enter a valid date.");
            //     }
            // }


            // public void DeleteEnergyGoal()
            // {
            //     AnsiConsole.Markup("[bold red]🗑 Delete Energy Goal[/]\n");
            //     AnsiConsole.WriteLine("==============================");

            //     // 📅 Request end date with validation
            //     var targetDate = AnsiConsole.Prompt(
            //         new TextPrompt<DateTime>("Enter the end date of the goal to delete (yyyy-MM-dd):")
            //             .Validate(input => input.Year > 2000 
            //                 ? ValidationResult.Success() 
            //                 : ValidationResult.Error("[red]❌ Invalid date format. Please enter a valid date.[/]"))
            //     );

            //     bool success = goalManager.DeleteGoal(targetDate);

            //     if (success)
            //     {
            //         AnsiConsole.Markup("[green]✅ Energy goal deleted successfully![/]\n");
            //     }
            //     else
            //     {
            //         AnsiConsole.Markup("[yellow]⚠ No goal found matching the provided date.[/]\n");
            //     }
            // }
            

        public void DeleteEnergyGoal()
        {
            if (goalManager == null || goalManager.Goals == null || goalManager.Goals.Count == 0)
            {
                AnsiConsole.Markup("[yellow]⚠ No active goals available for deletion.[/]\n");
                return;
            }

            AnsiConsole.Markup("[bold red]🗑 Delete Energy Goal[/]\n");
            AnsiConsole.WriteLine("==============================");

            // ✅ Create a list of formatted goal choices for selection
            var goalChoices = goalManager.Goals
                .Select(g => $"{g.GoalStartDate:yyyy-MM-dd} → {g.GoalEndDate:yyyy-MM-dd} ({g.ReductionGoal}%)")
                .ToList();

            // ✅ Prompt user to select a goal to delete
            var selectedGoal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Select the goal you want to delete:[/]")
                    .AddChoices(goalChoices)
            );

            // ✅ Extract the end date from the selected goal
            DateTime targetDate = DateTime.Parse(selectedGoal.Split('→')[1].Trim().Split(' ')[0]);

            // ✅ Delete the selected goal
            bool success = goalManager.DeleteGoal(targetDate);

            if (success)
            {
                AnsiConsole.Markup("[green]✅ Goal deleted successfully![/]\n");
            }
            else
            {
                AnsiConsole.Markup("[yellow]⚠ No matching goal found.[/]\n");
            }
        }



        

            private void ExitApplication()
            {
                Console.WriteLine("Thank you for using the Power Usage Application. Goodbye!");
                Environment.Exit(0);
            }
    }
}
