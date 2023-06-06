using System.Diagnostics;
using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;

namespace CGI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

        public async Task<IActionResult> GetUserId()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var userInfo = new AccountController(null).GetAuth0UserInfo(idToken);
            string userId = userInfo.UserId;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT User_ID, FullName FROM Users WHERE UUID = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                int userIdFromDb = reader.GetInt32(0);
                                string fullName = reader.GetString(1);
                                return Json(new { success = true, userId = userIdFromDb, fullName = fullName });
                            }
                        }
                    }
                }
            }

            return Json(new { success = false });
        }



        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
             _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [Authorize]
        public IActionResult Index(int lowerbound, int upperbound)
        {
            List<LeaderboardViewModel> leaderboardViewModels = new List<LeaderboardViewModel>();
            if (upperbound == 0)
            {
                upperbound = 500;
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlSelectUsers = "SELECT Users.Score, FullName, SUM(journeys.total_distance) AS total_user_distance, Users.User_ID FROM Users, (SELECT User_ID, SUM(total_distance) AS total_distance FROM Journeys GROUP BY User_ID) journeys WHERE Users.User_ID = journeys.User_ID AND total_distance > @lowerbounddistance AND total_distance < @upperbounddistance GROUP BY Users.User_ID, FullName, Users.Score ORDER BY Users.Score DESC";

                using (SqlCommand command = new SqlCommand(sqlSelectUsers, connection))
                {
                    command.Parameters.AddWithValue("@lowerbounddistance", lowerbound);
                    command.Parameters.AddWithValue("@upperbounddistance", upperbound);
                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        LeaderboardViewModel leaderboardViewModel = new LeaderboardViewModel();

                        if (!reader.IsDBNull(reader.GetOrdinal("Fullname")))
                        {
                            leaderboardViewModel.userName = (string)reader["Fullname"];
                            
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("Score")))
                        {
                            leaderboardViewModel.score = (int)reader["Score"];
                            leaderboardViewModel.lowerbound = lowerbound;
                            leaderboardViewModel.upperbound = upperbound;
                            Console.WriteLine(leaderboardViewModel.userName);
                        }

                        leaderboardViewModels.Add(leaderboardViewModel);
                    }


                    reader.Close();
                }

            }
            Console.WriteLine(lowerbound);
            Console.WriteLine(upperbound);
            return View(leaderboardViewModels);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}