namespace CGI.Models
{
    public enum Vehicle_Emission
    {
        Walking = 0,
        Bicycle = 0,
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
    }


    public class Stopover
    {
        public int Stopover_ID { get; set; }
        public Vehicle_ID VehicleType { get; set; }
        public int JourneyID { get; set; }
        public int Distance { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Emission { get; set; }


        private static readonly Dictionary<Vehicle_ID, Vehicle_Emission> s_emissionMap = new Dictionary<Vehicle_ID, Vehicle_Emission>
        {
            { Vehicle_ID.Walking, Vehicle_Emission.Walking },
            { Vehicle_ID.Bicycle, Vehicle_Emission.Bicycle },
            { Vehicle_ID.ETrain, Vehicle_Emission.ETrain },
            { Vehicle_ID.EBicycle, Vehicle_Emission.EBicycle },
            { Vehicle_ID.EScooter, Vehicle_Emission.EScooter },
            { Vehicle_ID.ECar, Vehicle_Emission.ECar },
            { Vehicle_ID.GScooter, Vehicle_Emission.GScooter },
            { Vehicle_ID.DTrain, Vehicle_Emission.DTrain },
            { Vehicle_ID.Tram, Vehicle_Emission.Tram },
            { Vehicle_ID.Bus, Vehicle_Emission.Bus },
            { Vehicle_ID.Metro, Vehicle_Emission.Metro },
            { Vehicle_ID.HCar, Vehicle_Emission.HCar },
            { Vehicle_ID.GMotorcycle, Vehicle_Emission.GMotorcycle },
            { Vehicle_ID.DCar, Vehicle_Emission.DCar },
            { Vehicle_ID.GCar, Vehicle_Emission.GCar }
        };

        public void CalculateEmission()
        {
            if (s_emissionMap.TryGetValue(VehicleType, out Vehicle_Emission vehicleEmission))
            {
                Emission = Distance * (int)vehicleEmission;
            }
            else
            {
                throw new Exception("Vehicle_ID error");
            }
        }
    }
}