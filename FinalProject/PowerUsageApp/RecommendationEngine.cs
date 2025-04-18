using System;
using System.Collections.Generic;

namespace PowerUsageApp
{
    public class RecommendationEngine
    {
        // Method to display general energy-saving tips
        public void DisplayGeneralTips()
        {
            Console.WriteLine("================================");
            Console.WriteLine("General Energy-Saving Tips");
            Console.WriteLine("================================");
            List<string> tips = GetGeneralTips();

            foreach (string tip in tips)
            {
                Console.WriteLine($"- {tip}");
            }
        }

        // Method to display custom recommendations based on data
        public void DisplayCustomTips(EnergyData recentData)
        {
            Console.WriteLine("================================");
            Console.WriteLine("Custom Recommendations");
            Console.WriteLine("================================");

            List<string> customTips = GetCustomTips(recentData);

            if (customTips.Count == 0)
            {
                Console.WriteLine("No custom recommendations available based on recent energy data.");
            }
            else
            {
                foreach (string tip in customTips)
                {
                    Console.WriteLine($"- {tip}");
                }
            }
        }

        // Private method to return general tips
        private List<string> GetGeneralTips()
        {
            return new List<string>
            {
                "Unplug devices when not in use to reduce standby power consumption.",
                "Switch to LED bulbs, which consume significantly less energy than traditional bulbs.",
                "Use smart thermostats to optimize heating and cooling settings.",
                "Run appliances like dishwashers and washing machines during off-peak hours.",
                "Ensure windows and doors are properly sealed to prevent energy loss."
            };
        }

        // Private method to return custom tips based on recent energy data
        private List<string> GetCustomTips(EnergyData data)
        {
            List<string> tips = new List<string>();

            // Analyze energy usage and provide targeted tips
            if (data.Usage > 50)
            {
                tips.Add("Consider upgrading appliances to energy-efficient models.");
            }
            if (data.Cost > 100)
            {
                tips.Add("Investigate energy provider options for potential cost savings.");
            }
            if (data.Usage < 10)
            {
                tips.Add("Your energy usage is already low! Keep up the good work.");
            }

            return tips;
        }
    }
}