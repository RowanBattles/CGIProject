﻿using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;


namespace CGI.Controllers
{
    public class JourneyController : Controller
    {
        private readonly string _connectionString;

        public JourneyController(IConfiguration configuration)
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

                string sql = "SELECT * FROM Journeys";

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
                            Date = ((DateTime)reader["Date"]).Date
                        };

                        journeys.Add(journey);
                    }

                    reader.Close();
                }
            }

            // Pass the list of journeys to the view
            return View(journeys);
        }

        public IActionResult Details(int id)
        {
            // Fetch a specific journey from the database
            Journey journey = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Journeys WHERE Journey_ID = @JourneyID";
 

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JourneyID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        journey = new Journey
                        {
                            JourneyID = (int)reader["Journey_ID"],
                            UserID = (int)reader["User_ID"],
                            TotalDistance = (int)reader["Total_Distance"],
                            TotalEmission = (int)reader["Total_Emission"],
                            Start = (string)reader["Start"],
                            End = (string)reader["End"],
                            Date = ((DateTime)reader["Date"]).Date
                        };
                    }

                    reader.Close();
                }
            }
            

                // If the journey is not found, return a 404 error
                if (journey == null)
            {
                return NotFound();
            }

            // Pass the journey to the view
            return View(journey);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Journey journey)
        {
            // Insert a new journey into the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Journeys (User_ID, Total_Distance, Total_Emission, Start, [End], Date) VALUES (@UserID, @TotalDistance, @TotalEmission, @Start, @End, @Date); SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", journey.UserID);
                    command.Parameters.AddWithValue("@TotalDistance", journey.TotalDistance);
                    command.Parameters.AddWithValue("@TotalEmission", journey.TotalEmission);
                    command.Parameters.AddWithValue("@Start", journey.Start);
                    command.Parameters.AddWithValue("@End", journey.End);
                    command.Parameters.AddWithValue("@Date", journey.Date);

                    // Execute the query and get the newly inserted Journey ID
                    int journeyID = Convert.ToInt32(command.ExecuteScalar());

                    // Set the JourneyID property of the journey object to the newly inserted ID
                    journey.JourneyID = journeyID;
                }
            }

            // Redirect to the Details action to show the newly created journey
            return RedirectToAction(nameof(Details), new { id = journey.JourneyID });
        }

        public IActionResult Edit(int id)
        {
            // Fetch a specific journey from the database
            Journey journey = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Journeys WHERE Journey_ID = @JourneyID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JourneyID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        journey = new Journey
                        {
                            JourneyID = (int)reader["Journey_ID"],
                            UserID = (int)reader["User_ID"],
                            TotalDistance = (int)reader["Total_Distance"],
                            TotalEmission = (int)reader["Total_Emission"],
                            Start = (string)reader["Start"],
                            End = (string)reader["End"],
                            Date = ((DateTime)reader["Date"]).Date
                        };
                    }

                    reader.Close();
                }
            }

            // If the journey is not found, return a 404 error
            if (journey == null)
            {
                return NotFound();
            }

            // Pass the journey to the view
            return View(journey);
        }

        [HttpPost]
        public IActionResult Edit(int id, Journey journey)
        {
            // Update an existing journey in the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "UPDATE Journeys SET User_ID = @UserID, Total_Distance = @TotalDistance, Total_Emission = @TotalEmission, Start = @Start, [End] = @End, Date = @Date WHERE Journey_ID = @JourneyID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JourneyID", id);
                    command.Parameters.AddWithValue("@UserID", journey.UserID);
                    command.Parameters.AddWithValue("@TotalDistance", journey.TotalDistance);
                    command.Parameters.AddWithValue("@TotalEmission", journey.TotalEmission);
                    command.Parameters.AddWithValue("@Start", journey.Start);
                    command.Parameters.AddWithValue("@End", journey.End);
                    command.Parameters.AddWithValue("@Date", journey.Date);

                    command.ExecuteNonQuery();
                }
            }

            // Redirect to the Details action to show the updated journey
            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Delete(int id)
        {
            // Fetch a specific journey from the database
            Journey journey = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Journeys WHERE Journey_ID = @JourneyID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JourneyID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        journey = new Journey
                        {
                            JourneyID = (int)reader["Journey_ID"],
                            UserID = (int)reader["User_ID"],
                            TotalDistance = (int)reader["Total_Distance"],
                            TotalEmission = (int)reader["Total_Emission"],
                            Start = (string)reader["Start"],
                            End = (string)reader["End"],
                            Date = ((DateTime)reader["Date"]).Date
                        };
                    }

                    reader.Close();
                }
            }

            // If the journey is not found, return a 404 error
            if (journey == null)
            {
                return NotFound();
            }

            // Pass the journey to the view
            return View(journey);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete an existing journey from the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Journeys WHERE Journey_ID = @JourneyID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JourneyID", id);

                    command.ExecuteNonQuery();
                }
            }

            // Redirect to the Index action to show the updated list of journeys
            return RedirectToAction(nameof(Index));
        }
    }
}
