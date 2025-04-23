using System;

namespace PowerUsageApp
{
    public class GoalManager
    {
        
        public double ReductionGoal { get; private set; } 
        public DateTime GoalStartDate { get; private set; }
        public DateTime GoalEndDate { get; private set; } 
        private double TotalEnergyUsageDuringGoal { get; set; }

        
        public void SetGoal(double reductionGoal, DateTime startDate, DateTime endDate)
        {
            
            if (startDate >= endDate)
            {
                Console.WriteLine("Error: Start date must be earlier than the end date.");
                return;
            }
            
            ReductionGoal = reductionGoal;
            GoalStartDate = startDate;
            GoalEndDate = endDate;
            TotalEnergyUsageDuringGoal = 0; // Reset tracking

            Console.WriteLine($"Goal set: Reduce energy by {ReductionGoal}% from {GoalStartDate.ToShortDateString()} to {GoalEndDate.ToShortDateString()}.");
        }
        
        public void TrackUsage(double dailyUsage, DateTime recordDate)
        {
            if (recordDate < GoalStartDate)
            {
                Console.WriteLine($"Record date {recordDate.ToShortDateString()} is before goal start. Ignoring.");
                return;
            }

            if (recordDate > GoalEndDate)
            {
                Console.WriteLine($"Record date {recordDate.ToShortDateString()} is after goal end. Ignoring.");
                return;
            }

            
            TotalEnergyUsageDuringGoal += dailyUsage;
            Console.WriteLine($"Tracking {dailyUsage} kWh on {recordDate.ToShortDateString()}. Total tracked: {TotalEnergyUsageDuringGoal} kWh.");
        }
        
        public double CalculateProgress(double baselineUsage)
        {
            if (baselineUsage <= 0)
            {                
                return 0;
            }            
            double reductionAchieved = ((baselineUsage - TotalEnergyUsageDuringGoal) / baselineUsage) * 100;
            reductionAchieved = Math.Max(0, reductionAchieved); // Ensure no negative values

            return reductionAchieved;
        }

        
        public bool IsGoalAchieved(double baselineUsage)
        {
            double progress = CalculateProgress(baselineUsage);
            return progress >= ReductionGoal; 
        }

        public void DisplayGoals()
        {
            Console.WriteLine("================================");
            Console.WriteLine("Your Active Energy Goals");
            Console.WriteLine("================================");

            if (ReductionGoal == 0)
            {
                Console.WriteLine("No active goals set. Please create an energy goal first.");
                return;
            }

            Console.WriteLine($"Goal: Reduce energy by {ReductionGoal}%");
            Console.WriteLine($"Period: {GoalStartDate.ToShortDateString()} to {GoalEndDate.ToShortDateString()}");
            //Console.WriteLine($" Energy Usage Tracked: {TotalEnergyUsageDuringGoal} kWh");
            Console.WriteLine("--------------------------------");


        }
    }
}