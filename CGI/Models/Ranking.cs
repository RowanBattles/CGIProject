namespace CGI.Models
{
    public class Ranking
    {
        private List<User> UserList { get; set; } = new();

        public List<User> SortList(List<User> UserList)
        {
            return UserList;
        }
        private void DisplayTopTen()
        {
            
        }
    }
}
