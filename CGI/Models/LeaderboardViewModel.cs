namespace CGI.Models
{
    public class LeaderboardViewModel
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public int score { get; set; }
        public bool UserIsGroupAdmin { get; set; }
        public int lowerbound { get; set; }
        public int upperbound { get; set; }
    }
}
