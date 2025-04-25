using System.Text.Json.Serialization;

    public class EnergyGoal
    {
        public double ReductionGoal { get; set; }
        public DateTime GoalStartDate { get; set; }
        public DateTime GoalEndDate { get; set; }
        public double TotalEnergyUsageDuringGoal { get; set; }


        public EnergyGoal() { }

        
        [JsonConstructor]
        public EnergyGoal(double reductionGoal, DateTime goalStartDate, DateTime goalEndDate, double totalEnergyUsageDuringGoal)
        {
            ReductionGoal = reductionGoal;
            GoalStartDate = goalStartDate;
            GoalEndDate = goalEndDate;
            TotalEnergyUsageDuringGoal = totalEnergyUsageDuringGoal;
        }


        public void TrackUsage(double dailyUsage, DateTime recordDate)
        {
            if (recordDate < GoalStartDate)
            {
                return;
            }

            if (recordDate > GoalEndDate)
            {
                return;
            }

            TotalEnergyUsageDuringGoal += dailyUsage;
            
        }

        public double CalculateProgress(double baselineUsage)
        {
            if (baselineUsage <= 0)
            {
                Console.WriteLine("❌ Baseline usage missing or invalid. Cannot calculate progress.");
                return -1;
            }


            if (TotalEnergyUsageDuringGoal > baselineUsage)
            {
                return 0; 
            }

            double reductionAchieved = ((baselineUsage - TotalEnergyUsageDuringGoal) / baselineUsage) * 100;
            return Math.Max(0, Math.Min(100, reductionAchieved)); 
        }

        
        public bool IsGoalAchieved(double baselineUsage)
        {
            return CalculateProgress(baselineUsage) >= ReductionGoal;
        }
    }

