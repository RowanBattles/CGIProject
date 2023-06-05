namespace CGI.Models
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public bool UserIsPartOfGroup { get; set; } = false;
        public bool UserIsGroupAdmin { get; set; } = false;
        public List<Group> Groups { get; set; }
        public List<LeaderboardViewModel> Leaderboards { get; set; } = new List<LeaderboardViewModel>();
    }
}