namespace CGI.Models
{
    public class Journey
    {
        public int JourneyID { get; set; }
        public int UserID { get; set; }
        public int TotalDistance { get; set; }
        public int TotalEmission { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public DateTime Date { get; set; }
        public List<Stopover> Stopovers { get; set; }
    }
}
