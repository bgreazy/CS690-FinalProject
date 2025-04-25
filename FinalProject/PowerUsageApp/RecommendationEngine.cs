using System;
using System.Collections.Generic;

namespace PowerUsageApp
{
    public class RecommendationEngine
    {        
        public void DisplayGeneralTips()
        {
            // Console.WriteLine("================================");
            // Console.WriteLine("General Energy-Saving Tips");
            // Console.WriteLine("================================");
            List<string> tips = GetGeneralTips();

            foreach (string tip in tips)
            {
                Console.WriteLine($"- {tip}");
            }
        }
        
        public void DisplayCustomTips(EnergyData recentData)
        {
            // Console.WriteLine("================================");
            // Console.WriteLine("Custom Recommendations");
            // Console.WriteLine("================================");

            List<string> customTips = GetCustomTips(recentData);

            if (recentData.Usage > 50) // Example threshold for high usage
            {
                Console.WriteLine("- Your recent energy usage is quite high! Consider adjusting appliance usage or investing in energy-efficient devices.");
            }

            if (recentData.Cost / recentData.Usage > 0.20) // Example threshold for high cost per kWh
            {
                Console.WriteLine("- Your cost per kWh is above average. Check for better energy rate plans.");
            }

            Console.WriteLine("- Try using smart thermostats or automated energy-saving tools for optimization.");
        }
        
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

        private List<string> GetCustomTips(EnergyData data)
        {
            List<string> tips = new List<string>();

            
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