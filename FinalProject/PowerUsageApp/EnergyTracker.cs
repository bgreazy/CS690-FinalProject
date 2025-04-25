using System.Text.Json;
using Spectre.Console;

namespace PowerUsageApp
{
    public class EnergyTracker
    {        
        private List<EnergyData> Records;
        private DataStorage storage;
        private readonly string filePath; 

        public EnergyTracker(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "File path cannot be empty.");
            }

            this.filePath = filePath;
            storage = new DataStorage(filePath);
            
            
            if (Records == null || Records.Count == 0) 
            {
                Records = storage.LoadData() ?? new List<EnergyData>();
            }

        }

        public void AddEntry(EnergyData data, bool showMessage = true)
        {
            // Console.WriteLine("🔍 DEBUG: Checking for duplicates before adding entry");

            if (Records.Any(r => r.Date == data.Date && r.Usage == data.Usage && r.Cost == data.Cost))
            {
                Console.WriteLine("⚠ Duplicate entry detected. Skipping save.");
                return;
            }

            Records.Add(data);            
        }

        public void SaveEnergyData()
        {
            storage.SaveData(Records);
            Console.WriteLine("✅ Energy data saved successfully.");
        }

        public List<EnergyData> LoadData()
        {
            

            if (!File.Exists(filePath))
            {
                Console.WriteLine("ℹ No saved energy data found.");
                return new List<EnergyData>();  
            }

            try
            {
                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("⚠ Warning: File is empty. Starting with a fresh record list.");
                    return new List<EnergyData>();
                }

                var loadedRecords = JsonSerializer.Deserialize<List<EnergyData>>(json) ?? new List<EnergyData>();

                Console.WriteLine($"✅ Loaded {loadedRecords.Count} records from storage.");

                return loadedRecords;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading energy data: {ex.Message}");
                return new List<EnergyData>();
            }
        }

        public double CalculateTotalUsageForDay(DateTime day)
        {
            var recordsForDay = Records.Where(record => record.Date.Date == day.Date).ToList();
            
            if (recordsForDay.Count == 0)
            {
                Console.WriteLine($"⚠ No records found for {day.ToShortDateString()}.");
                return 0;
            }

            double totalUsage = recordsForDay.Sum(record => record.Usage);
            Console.WriteLine($"🔍 Total usage for {day.ToShortDateString()}: {totalUsage} kWh");
            return totalUsage;
        }



        public void DisplayInsights()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[bold cyan]📊 Energy Usage Insights[/]\n");
            AnsiConsole.WriteLine("==============================");

            var table = new Table();
            table.AddColumn("📅 Date");
            table.AddColumn("⚡ Usage");
            table.AddColumn("💲 Cost");
            table.AddColumn("📈 Total");

            foreach (var record in Records)
            {
                double total = record.Usage * record.Cost;
                table.AddRow(
                    record.Date.ToString("yyyy-MM-dd"),
                    $"{record.Usage} kWh",
                    $"${record.Cost:F2}",
                    $"${total:F2}"
                );
            }

            AnsiConsole.Write(table);
        }


        public List<EnergyData> GetAllRecords()
        {
            return Records ?? new List<EnergyData>();
        }

        public void DeleteEnergyData()
        {
            if (this.GetAllRecords().Count == 0)
            {
                AnsiConsole.Markup("[yellow]⚠ No energy records available for deletion.[/]\n");
                return;
            }

            AnsiConsole.Markup("[bold red]🗑 Delete Energy Data[/]\n");
            AnsiConsole.WriteLine("==============================");

            var records = this.GetAllRecords(); // ✅ No need for `tracker`

            // ✅ Create formatted choices for selection
            var recordChoices = records.Select(r => $"{r.Date:yyyy-MM-dd} | {r.Usage} kWh | ${r.Cost:F2}").ToList();

            // ✅ Prompt user to select an entry for deletion
            var selectedRecord = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Select an energy entry to delete:[/]")
                    .AddChoices(recordChoices)
            );

            // ✅ Extract date from selection and remove the record
            DateTime targetDate = DateTime.Parse(selectedRecord.Split('|')[0].Trim());
            var recordToRemove = records.FirstOrDefault(r => r.Date == targetDate);

            if (recordToRemove != null)
            {
                records.Remove(recordToRemove);
                this.SaveEnergyData(); // ✅ Use `this` instead of `tracker`

                AnsiConsole.Markup("[green]✅ Energy data entry deleted successfully.[/]\n");
            }
            else
            {
                AnsiConsole.Markup("[yellow]⚠ No matching record found.[/]\n");
            }
        }

        
    }
}