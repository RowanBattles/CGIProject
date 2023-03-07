using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CGI.Controllers
{
    public class UserController : Controller
    {
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            // Fetch all journeys from the database
            List<Journey> journeys = new List<Journey>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Users";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Journey journey = new Journey
                        {
                            JourneyID = (int)reader["Journey_ID"],
                            UserID = (int)reader["User_ID"],
                            TotalDistance = (int)reader["Total_Distance"],
                            TotalEmission = (int)reader["Total_Emission"],
                            Start = (string)reader["Start"],
                            End = (string)reader["End"],
                            Date = (DateTime)reader["Date"]
                        };

                        journeys.Add(journey);
                    }

                    reader.Close();
                }
            }

            // Pass the list of journeys to the view
            return View(journeys);
        }
    }
}
