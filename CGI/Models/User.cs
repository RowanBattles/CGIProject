namespace CGI.Models
{
    public class User
    {
        public string UserId { get; set; }
        public int Score { get; set; }
        public string Name { get; set; }

        // Score = 500 - (( List<Stopover> ( Distance * Emission ) / ( Journey ( Total_Distance * EmissionMax )) * 500 )
    }
}
