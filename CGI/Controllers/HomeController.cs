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
                using (SqlCommand cmd = new SqlCommand("SELECT User_ID FROM Users WHERE UUID = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                    {
                        int userIdFromDb = (int)result;
                        return Json(new { success = true, userId = userIdFromDb });
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
        public IActionResult Index()
        {
            List<LeaderboardViewModel> leaderboardViewModels = new List<LeaderboardViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlSelectUsers = "SELECT u.User_ID, u.FullName, SUM(j.Total_Emission) AS UserEmission FROM Journeys j, Users u WHERE j.User_ID = u.User_ID GROUP BY u.User_ID, u.FullName ORDER BY UserEmission ASC";

                using (SqlCommand command = new SqlCommand(sqlSelectUsers, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        LeaderboardViewModel leaderboardViewModel = new LeaderboardViewModel
                        {
                            userName = (string)reader["Fullname"],
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
        public IActionResult UserTest()
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