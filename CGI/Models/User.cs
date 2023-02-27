namespace CGI.Models
{
    public class User
    {
        private int ID { get; set; }

        private int Score { get; set; }

        public User(int iD, int score)
        {
            this.ID = iD;
            this.Score = score;
        }
    }
}
