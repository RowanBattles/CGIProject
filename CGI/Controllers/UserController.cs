﻿using CGI.Models;
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
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Users";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User
                        {
                            UserId = reader["User_ID"].ToString(),
                            Name = reader["Fullname"].ToString()
                        };

                        users.Add(user);
                    }

                    reader.Close();
                }
            }

            return View(users);
        }

        public int CalculateScore()
        {
            int score = 0;

            // Select UserID
            User user = null;

            // Select every Journey for that user
            List<Journey> AllJourneys = null;

            foreach (Journey journey in AllJourneys)
            {
                score += journey.Score;
            }

            return score;
        }
    }
}
