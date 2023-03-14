namespace CGI.Models
{
    public class Journey
    {

        public int JourneyID { get; set; }
        public int UserID { get; set; }
        public int TotalDistance { get; set; }
        public int TotalEmission { get; set; }
<<<<<<< HEAD
        public string? Start { get; set; }
        public string? End { get; set; }
        public DateTime Date { get; set; }
        public List<Stopover> Stopovers { get; set; }
=======
        public string Start { get; set; }
        public string End { get; set; }
        public DateTime Date { get; set; }
        public List<Stopover> stopovers { get; set; }
>>>>>>> a4f40e23c65ad868cce056a116858172d8904f46
    }

}
