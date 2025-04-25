namespace PowerUsageApp
{
    public class EnergyData
    {
        
        public DateTime Date { get; set; }   
        public double Usage { get; set; }   
        public double Cost { get; set; }    

        public EnergyData(DateTime date, double usage, double cost)
        {
            Date = date;
            Usage = usage;
            Cost = cost;
        }

        
        public bool ValidateData()
        {            
            if (Usage < 0 || Cost < 0)
            {
                Console.WriteLine("Error: Usage and cost values must be non-negative.");
                return false;
            }            
            return true;
        }
        
        public override string ToString()
        {
            return $"Date: {Date.ToShortDateString()}, Usage: {Usage} kWh, Cost: ${Cost}";
        }
    }
}