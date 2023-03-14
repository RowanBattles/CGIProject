namespace CGI.Models
{
    enum Vehicle
    {
        Car = 1,
        Bike = 2,
        Bus = 3,
        Train = 4,
        Plane = 5
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
        public int CalculateEmission(int Emission, int Distance)
        {
            //Work in progress..
            return Emission * Distance;
        }
        public Journey Journey { get; set; } //Add this property to hold the Journey information
    }
}
