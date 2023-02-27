namespace CGI.Models
{
    public class Stopover
    {
        private int Distance { get; set; }
        private int Emission { get; set; }
        enum Vehicle
        {
            Car,
            Bike,
            //More examples added soon
        };
        private int Start { get; set; }
        private int End { get; set; }   
        public int CalculateEmission(int Emission, int Distance)
        {
            //Work in progress..
            return Emission * Distance;
        }
    }
}
