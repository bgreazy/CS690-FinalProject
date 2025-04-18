using System;

namespace PowerUsageApp
{
    public class GoalManager
    {
        // Attributes
        public double ReductionGoal { get; private set; }       // Reduction goal (e.g., percentage or fixed value)
        public DateTime GoalStartDate { get; private set; }     // Start date for the goal
        public DateTime GoalEndDate { get; private set; }       // End date for the goal
        private double TotalEnergyUsageDuringGoal { get; set; } // Energy usage tracked during the goal period

        // Method to set a new goal
        public void SetGoal(double reductionGoal, DateTime startDate, DateTime endDate)
        {
            // Validate dates
            if (startDate >= endDate)
            {
                Console.WriteLine("Error: Start date must be earlier than the end date.");
                return;
            }

            // Set goal attributes
            ReductionGoal = reductionGoal;
            GoalStartDate = startDate;
            GoalEndDate = endDate;
            TotalEnergyUsageDuringGoal = 0; // Reset tracking

            Console.WriteLine($"Goal set: Reduce energy by {ReductionGoal}% from {GoalStartDate.ToShortDateString()} to {GoalEndDate.ToShortDateString()}.");
        }

        // Method to track energy usage during the goal period
        public void TrackUsage(double dailyUsage)
        {
            // Check if the current date is within the goal period
            DateTime today = DateTime.Now;
            if (today >= GoalStartDate && today <= GoalEndDate)
            {
                TotalEnergyUsageDuringGoal += dailyUsage;
                Console.WriteLine($"Tracked {dailyUsage} kWh usage. Total during goal period: {TotalEnergyUsageDuringGoal} kWh.");
            }
            else
            {
                Console.WriteLine("Current date is outside the goal period. Usage not tracked.");
            }
        }

        // Method to calculate progress toward the goal
        public double CalculateProgress(double baselineUsage)
        {
            if (baselineUsage <= 0)
            {
                Console.WriteLine("Error: Baseline usage must be greater than zero.");
                return 0;
            }

            // Calculate reduction percentage
            double reductionAchieved = ((baselineUsage - TotalEnergyUsageDuringGoal) / baselineUsage) * 100;
            reductionAchieved = Math.Max(0, reductionAchieved); // Ensure no negative values

            Console.WriteLine($"Progress toward goal: {reductionAchieved:F2}% achieved.");
            return reductionAchieved;
        }

        // Method to check if the goal has been achieved
        public bool IsGoalAchieved(double baselineUsage)
        {
            double progress = CalculateProgress(baselineUsage);
            if (progress >= ReductionGoal)
            {
                Console.WriteLine("Congratulations! You have achieved your energy reduction goal.");
                return true;
            }
            else
            {
                Console.WriteLine("Goal not yet achieved. Keep going!");
                return false;
            }
        }
    }
}