namespace CGI.Models
{
    public enum Vehicle
    {
        Walking = 1,
        Bicycle,
        ETrain, 
        EBicycle,
        EScooter,
        ECar,
        GScooter,
        DTrain,
        Tram,
        Bus,
        Metro,
        HCar,
        GMotorcycle,
        DCar,
        GCar,

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