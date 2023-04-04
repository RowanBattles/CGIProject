﻿using CGI.Models;
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

        [HttpPost]
        public async Task<IActionResult> Create(Journey journey)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Journeys (User_ID, Total_Distance, Total_Emission, Start, End, Date) VALUES (@User_ID, @Total_Distance, @Total_Emission, @Start, @End, @Date)", conn))
                {
                    cmd.Parameters.AddWithValue("@User_ID", journey.User_ID);
                    cmd.Parameters.AddWithValue("@Total_Distance", journey.Total_Distance);
                    cmd.Parameters.AddWithValue("@Total_Emission", journey.Total_Emission);
                    cmd.Parameters.AddWithValue("@Start", journey.Start);
                    cmd.Parameters.AddWithValue("@End", journey.End);
                    cmd.Parameters.AddWithValue("@Date", journey.Date);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            List<Journey> journeys = new List<Journey>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Journeys", conn))
                {
                    
                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Journey journey = new Journey
                            {
                                Journey_ID = reader.GetInt32(0),
                                User_ID = reader.GetInt32(1),
                                Total_Distance = reader.GetInt32(2),
                                Total_Emission = reader.GetInt32(3),
                                Start = reader.GetString(4),
                                End = reader.GetString(5),
                                Date = reader.GetDateTime(6)
                            };
                            journeys.Add(journey);
                        }
                    }
                }
            }

            string id = Request.Query["id"];

            if (string.IsNullOrEmpty(id))
            {
                id = "0";
            }
            

            List<Stopover> stopovers = new List<Stopover>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Stopovers WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", id);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Stopover stopover = new Stopover
                            {
                                StopoverID = reader.GetInt32(0),
                                VehicleID = reader.GetInt32(1),
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

            JourneyViewModel model = new()
            {
                Id = int.Parse(id),
                Journeys = journeys,
                Stopovers = stopovers,
            };

            return View(model);
        }
        public async Task<IActionResult> DeleteJourneyWhenZeroStopovers(List<Stopover> stopovers)
        {
            if (stopovers.Count == 0)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Journeys WHERE Journey_ID = @Journey_ID", conn))
                    {
                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            return RedirectToAction("Index");

        }
        // Update
        public async Task<IActionResult> Edit(int id)
        {
            Journey journey;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Journeys WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", id);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                        {
                            return NotFound();
                        }

                        journey = new Journey
                        {
                            Journey_ID = reader.GetInt32(0),
                            User_ID = reader.GetInt32(1),
                            Total_Distance = reader.GetInt32(2),
                            Total_Emission = reader.GetInt32(3),
                            Start = reader.GetString(4),
                            End = reader.GetString(5),
                            Date = reader.GetDateTime(6)
                        };
                    }
                }
            }

            return View(journey);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Journey journey)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
        {
                using (SqlCommand cmd = new SqlCommand("UPDATE Journeys SET User_ID = @User_ID, Total_Distance = @Total_Distance, Total_Emission = @Total_Emission, Start = @Start, End = @End, Date = @Date WHERE Journey_ID = @Journey_ID", conn))
                {
                    cmd.Parameters.AddWithValue("@Journey_ID", journey.Journey_ID);
                    cmd.Parameters.AddWithValue("@User_ID", journey.User_ID);
                    cmd.Parameters.AddWithValue("@Total_Distance", journey.Total_Distance);
                    cmd.Parameters.AddWithValue("@Total_Emission", journey.Total_Emission);
                    cmd.Parameters.AddWithValue("@Start", journey.Start);
                    cmd.Parameters.AddWithValue("@End", journey.End);
                    cmd.Parameters.AddWithValue("@Date", journey.Date);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Journeys WHERE Journey_ID = @Journey_ID", conn))
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
