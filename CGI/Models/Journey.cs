namespace CGI.Models
{
    public class Journey
    {
        private int Total_Distance { get; set; }
        private int Total_Emission { get; set; }
        private int Date { get; set; }
        private List<Stopover> AllStopOvers = new List<Stopover>();
        private int User_ID { get; set; }
        public Journey GetJourney(Journey journey)
        {
            return journey;   
        }
    }
}
