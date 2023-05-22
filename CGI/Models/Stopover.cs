using System.ComponentModel.DataAnnotations;

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
    }

    public enum Vehicle_ID
    {
        [Display(Name = "Walking")]
        Walking = 1,
        [Display(Name = "Bicycle")]
        Bicycle = 2,
        [Display(Name = "Electric Train")]
        ETrain = 3,
        [Display(Name = "Electric Bicycle")]
        EBicycle = 4,
        [Display(Name = "Electric Scooter")]
        EScooter = 5,
        [Display(Name = "Electric Car")]
        ECar = 6,
        [Display(Name = "Gasoline Scooter")]
        GScooter = 7,
        [Display(Name = "Diesel Train")]
        DTrain = 8,
        [Display(Name = "Tram")]
        Tram = 9,
        [Display(Name = "Bus")]
        Bus = 10,
        [Display(Name = "Metro")]
        Metro = 11,
        [Display(Name = "Hybrid Car")]
        HCar = 12,
        [Display(Name = "Gasoline Motorcycle")]
        GMotorcycle = 13,
        [Display(Name = "Diesel Car")]
        DCar = 14,
        [Display(Name = "Gasoline Car")]
        GCar = 15,
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


        public static readonly Dictionary<Vehicle_ID, Vehicle_Emission> s_emissionMap =
             new()
             {
                {Vehicle_ID.Walking, Vehicle_Emission.Walking},
                {Vehicle_ID.Bicycle, Vehicle_Emission.Bicycle},
                {Vehicle_ID.ETrain, Vehicle_Emission.ETrain},
                {Vehicle_ID.EBicycle, Vehicle_Emission.EBicycle},
                {Vehicle_ID.EScooter, Vehicle_Emission.EScooter},
                {Vehicle_ID.ECar, Vehicle_Emission.ECar},
                {Vehicle_ID.GScooter, Vehicle_Emission.GScooter},
                {Vehicle_ID.DTrain, Vehicle_Emission.DTrain},
                {Vehicle_ID.Tram, Vehicle_Emission.Tram},
                {Vehicle_ID.Bus, Vehicle_Emission.Bus},
                {Vehicle_ID.Metro, Vehicle_Emission.Metro},
                {Vehicle_ID.HCar, Vehicle_Emission.HCar},
                {Vehicle_ID.GMotorcycle, Vehicle_Emission.GMotorcycle},
                {Vehicle_ID.DCar, Vehicle_Emission.DCar},
                {Vehicle_ID.GCar, Vehicle_Emission.GCar}
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