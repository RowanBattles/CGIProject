namespace CGI.Models
{
    public enum Vehicle_Emission
    {
        Walking = 1,
        Bicycle = 1,
        ETrain = 2,
        EBicycle = 3,
        EScooter = 17,
        ECar = 54,
        GScooter = 62,
        DTrain = 90,
        Tram = 96,
        Bus = 96,
        Metro = 96,
        HCar = 110,
        GMotorcycle = 129,
        DCar = 131,
        GCar = 149,

        // E = Electric
        // G = Gasoline
        // H = Hybrid
        // D = Diesel
    }

    public enum Vehicle_ID
    {
        Walking = 0,
        Bicycle = 1,
        ETrain = 2,
        EBicycle = 3,
        EScooter = 4,
        ECar = 5,
        GScooter = 6,
        DTrain = 7,
        Tram = 8,
        Bus = 9,
        Metro = 10,
        HCar = 11,
        GMotorcycle = 12,
        DCar = 13,
        GCar = 14,
    }

    public class Stopover
    {
        public int Stopover_ID { get; set; }
        public Vehicle_ID VehicleType { get; set; }
        public Vehicle_Emission VehicleEmission { get; set; }
        public int JourneyID { get; set; }
        public int Distance { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Emission { get; set; }

        public void CalculateEmission()
        {
            Emission = Distance * (int)VehicleType;
        }
    }
}