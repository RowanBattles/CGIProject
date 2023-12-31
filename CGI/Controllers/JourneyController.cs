﻿using System.Diagnostics.CodeAnalysis;
using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace CGI.Controllers
{
    public class JourneyController : Controller
    {
        private readonly string _connectionString;

        public JourneyController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAndGetJourneyId(int userId)
        {
            int newJourneyId;
            DateTime now = DateTime.Now;

            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("INSERT INTO Journeys (User_ID, Date) OUTPUT INSERTED.Journey_ID VALUES (@User_ID, @Date)", conn))
                {
                    cmd.Parameters.AddWithValue("@User_ID", userId);
                    cmd.Parameters.AddWithValue("@Date", now);

                    conn.Open();
                    newJourneyId = (int)await cmd.ExecuteScalarAsync();
                }
            }

            return Json(new { success = true, journeyId = newJourneyId });
        }



        [HttpPost]
        public async Task<IActionResult> SubmitJourney(int journeyId, int userId)
        {
            List<Stopover> stopovers = new();

            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("SELECT * FROM Stopovers WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", journeyId);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) &&
                                !reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5) && !reader.IsDBNull(6))
                            {
                                Stopover stopover = new()
                                {
                                    Stopover_ID = reader.GetInt32(0),
                                    VehicleType = (Vehicle_ID)reader.GetInt32(1),
                                    JourneyID = reader.GetInt32(2),
                                    Distance = reader.GetInt32(3),
                                    Start = reader.GetString(4),
                                    End = reader.GetString(5),
                                    Emission = reader.GetInt32(6)
                                };
                                stopovers.Add(stopover);
                            }
                        }
                    }
                }
            }

            // Calculate total distance and total emission from the stopovers
            int totalDistance = 0;
            int totalEmission = 0;

            foreach (var stopover in stopovers)
            {
                totalDistance += stopover.Distance;
                totalEmission += stopover.Emission;
            }

            int score = 500 - (int)(totalEmission / (double)(totalDistance * Enum.GetValues(typeof(Vehicle_Emission)).Cast<int>().Max()) * 500);

            // Get the Start and End from the first and last stopovers
            string start = stopovers[0].Start;
            string end = stopovers[^1].End;

            // Update the journey in the database
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("UPDATE Journeys SET Total_Distance = @Total_Distance, Score = @Score, Total_Emission = @Total_Emission, Start = @Start, [End] = @End WHERE Journey_ID = @Journey_ID AND User_ID = @User_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", journeyId);
                    cmd.Parameters.AddWithValue("@User_ID", userId);
                    cmd.Parameters.AddWithValue("@Total_Distance", totalDistance);
                    cmd.Parameters.AddWithValue("@Total_Emission", totalEmission);
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    cmd.Parameters.AddWithValue("@Score", score);

                    conn.Open();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return Json(new { success = false, message = "Failed to update journey" });
                    }
                }
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int JourneyID)
        {

            Journey journey;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Journeys WHERE Journey_ID = @Journeyid ", conn))
                {
                    cmd.Parameters.AddWithValue("@Journeyid", JourneyID);

                    conn.Open();



                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            journey = new Journey
                            {
                                Journey_ID = reader.GetInt32(0),
                                User_ID = reader.GetInt32(1),
                                Total_Distance = reader.GetInt32(2),
                                Total_Emission = reader.GetInt32(3),
                                Start = reader.GetString(4),
                                End = reader.GetString(5),
                                Score = reader.GetInt32(7),
                            };
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return View(journey);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateJourney(int journeyId, int userId, [FromBody] List<Stopover> stopovers)
        {
            // Calculate total distance and total emission from the stopovers
            int totalDistance = 0;
            int totalEmission = 0;

            foreach (var stopover in stopovers)
            {
                totalDistance += stopover.Distance;
                totalEmission += stopover.Emission;
            }

            int score = 500 - (int)(totalEmission / (double)(totalDistance * Enum.GetValues(typeof(Vehicle_Emission)).Cast<int>().Max()) * 500);

            // Get the Start and End from the first and last stopovers
            string start = stopovers[0].Start;
            string end = stopovers[^1].End;

            // Update the journey in the database
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("UPDATE Journeys SET Total_Distance = @Total_Distance, Score = @Score, Total_Emission = @Total_Emission, Start = @Start, [End] = @End WHERE Journey_ID = @Journey_ID AND User_ID = @User_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", journeyId);
                    cmd.Parameters.AddWithValue("@User_ID", userId);
                    cmd.Parameters.AddWithValue("@Total_Distance", totalDistance);
                    cmd.Parameters.AddWithValue("@Total_Emission", totalEmission);
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@End", end);
                    cmd.Parameters.AddWithValue("@Score", score);

                    conn.Open();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();


                    if (rowsAffected == 0)
                    {
                        return Json(new { success = false, message = "Failed to update journey" });
                    }
                }
            }

            // Add stopovers to the database
            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();

                foreach (var stopover in stopovers)
                {
                    using (SqlCommand cmd = new("INSERT INTO Stopovers (Vehicle_ID, Journey_ID, Distance, Start, [End], Emission) VALUES (@Vehicle_ID, @Journey_ID, @Distance, @Start, @End, @Emission)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Vehicle_ID", (int)stopover.VehicleType);
                        cmd.Parameters.AddWithValue("@Journey_ID", journeyId);
                        cmd.Parameters.AddWithValue("@Distance", stopover.Distance);
                        cmd.Parameters.AddWithValue("@Start", stopover.Start);
                        cmd.Parameters.AddWithValue("@End", stopover.End);
                        cmd.Parameters.AddWithValue("@Emission", stopover.Emission);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

            }

            return Json(new { success = true });
        }


        public async Task<IActionResult> Index()
        {
            List<Journey> journeys = new();

            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("SELECT * FROM Journeys", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) &&
                                !reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5) &&
                                !reader.IsDBNull(6) && !reader.IsDBNull(7))
                            {
                                Journey journey = new()
                                {
                                    Journey_ID = reader.GetInt32(0),
                                    User_ID = reader.GetInt32(1),
                                    Total_Distance = reader.GetInt32(2),
                                    Total_Emission = reader.GetInt32(3),
                                    Start = reader.GetString(4),
                                    End = reader.GetString(5),
                                    Date = reader.GetDateTime(6),
                                    Score = reader.GetInt32(7)
                                };
                                journeys.Add(journey);
                            }
                        }


                    }
                }
            }

            return View(journeys);
        }

        public IActionResult Details(int id)
        {
            List<Stopover> stopovers = new();
            Journey journey = null;

            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new("SELECT * FROM Stopovers WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Stopover stopover = new()
                            {
                                Stopover_ID = reader.GetInt32(0),
                                VehicleType = (Vehicle_ID)reader.GetInt32(1),
                                JourneyID = reader.GetInt32(2),
                                Distance = reader.GetInt32(3),
                                Start = reader.GetString(4),
                                End = reader.GetString(5),
                                Emission = reader.GetInt32(6)
                            };
                            stopovers.Add(stopover);
                        }
                    }
                }
            }

            using (SqlConnection conn = new(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new("SELECT * FROM Journeys WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            journey = new Journey();
                            journey.Journey_ID = reader.GetInt32(0);
                            journey.User_ID = reader.GetInt32(1);
                            journey.Total_Distance = reader.GetInt32(2);
                            journey.Total_Emission = reader.GetInt32(3);
                            journey.Start = reader.GetString(4);
                            journey.End = reader.GetString(5);
                            journey.Date = reader.GetDateTime(6);
                            journey.Score = reader.GetInt32(7);
                        }
                    }
                }
            }

            if (journey == null)
            {
                return RedirectToAction("Index", "Journey");
            }

            JourneyViewModel ViewModel = new();
            ViewModel.Journey = journey;
            ViewModel.Stopovers = stopovers;

            return View(ViewModel);
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("DELETE FROM Journeys WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", id);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
