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
            
            tracker.DisplayInsights(); 
            
        }        

        

        private void SetEnergyGoals()
        {
            AnsiConsole.Markup("[bold cyan]🏆 Set Energy Goals[/]\n");
            AnsiConsole.WriteLine("==============================");
            
            var goal = AnsiConsole.Prompt(
                new TextPrompt<double>("Enter reduction goal (e.g., 10% or 100 kWh):")
                    .Validate(input => input > 0 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]❌ Error: Please enter a valid positive goal.[/]"))
            );
            
            var startDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter start date (yyyy-MM-dd):")
                    .Validate(input => input.Year > 2000 
                        ? ValidationResult.Success() 
                        : ValidationResult.Error("[red]❌ Error: Invalid date format. Use yyyy-MM-dd.[/]"))
            );

            
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

            DisplayMenu();
        }
            

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
