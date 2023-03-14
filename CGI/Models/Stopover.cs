namespace CGI.Models
{
    public class Stopover
    {
        public string Distance { get; set; }
        public string Emission { get; set; }

        public string Vehicle { get; set;}

        // enum Vehicle
        // {
        //     Car,
        //     Bike,
        //     //More examples added soon
        // };
        public string Start { get; set; }
        public string End { get; set; }

        public int CalculateEmission(int Emission, int Distance)
        {
            //Work in progress..
            return Emission * Distance;
        }
    }
}
