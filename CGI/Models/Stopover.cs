namespace CGI.Models
{
    public enum Vehicle
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



    public class Stopover
    {
        public int Stopover_ID { get; set; }
        public Vehicle VehicleType { get; set; }
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