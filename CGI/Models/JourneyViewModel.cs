namespace CGI.Models
{
    public class JourneyViewModel
    {
        public int Id { get; set; }

        public Journey Journey { get; set; }
        public List<Journey> Journeys { get; set; }
        public List<Stopover> Stopovers { get; set; }
    }
}
