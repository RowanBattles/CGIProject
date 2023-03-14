using System.Diagnostics;
using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace CGI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

<<<<<<< HEAD
        
        public HomeController(ILogger<HomeController> logger)
=======
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
>>>>>>> a4f40e23c65ad868cce056a116858172d8904f46
        {
            _logger = logger;
             _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
<<<<<<< HEAD

=======
>>>>>>> a4f40e23c65ad868cce056a116858172d8904f46
        [Authorize]
        public IActionResult Index()
        {
            List<LeaderboardViewModel> leaderboardViewModels = new List<LeaderboardViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlSelectUsers = "SELECT User_id, SUM(Total_Emission) AS UserEmission FROM Journeys GROUP BY User_ID ORDER BY UserEmission ASC";

                using (SqlCommand command = new SqlCommand(sqlSelectUsers, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        LeaderboardViewModel leaderboardViewModel = new LeaderboardViewModel
                        {
                            userID = (int)reader["User_ID"],
                            score = (int)reader["UserEmission"]
                        };
                        leaderboardViewModels.Add(leaderboardViewModel);

                    }

                    reader.Close();
                }

            }

            return View(leaderboardViewModels);
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
<<<<<<< HEAD
        public IActionResult UserTest()
=======
        public IActionResult ManageJourney()
>>>>>>> a4f40e23c65ad868cce056a116858172d8904f46
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}