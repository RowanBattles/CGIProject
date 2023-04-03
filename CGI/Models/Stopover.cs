namespace CGI.Models
{
    public enum Vehicle
    {
        Walking = 0,
        Bicycle = 0,
        EBicycle = 3,
        HCar = 110,
        DCar = 131,
        GCar = 149,
        ECar = 54,
        EScooter = 17,
        GScooter = 62,
        DTrain = 90,
        ETrain = 2,
        Bus = 96,
        Tram = 96,
        Metro = 96,
        GMotorcycle = 129

        // E = Electric
        // G = Gasoline
        // H = Hybrid
        // D = Diesel
    }

    public class Stopover
    {
        public int StopoverID { get; set; }
        public int VehicleID { get; set; }
        public int JourneyID { get; set; }
        public int Distance { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Emission { get; set; }

        public void CalculateEmission()
        {
            Emission = Distance * VehicleID;
        }
    }
}