using CGI.Extensions;
using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Data;

namespace CGI.Controllers
{
    public class StopOverController : Controller
    {
        private readonly string _connectionString;
        public StopOverController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public async Task<IActionResult> CreateStopOver(Stopover stopover)
        {
            if (ModelState.IsValid)
            {
                int newStopoverId;
                stopover.CalculateEmission();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SqlCommand command = new SqlCommand("INSERT INTO Stopovers (Journey_ID, [Start], [End], Vehicle_ID, Distance, Emission) OUTPUT INSERTED.Stopover_ID VALUES (@JourneyId, @Start, @End, @VehicleType, @Distance, @Emission)", connection))
                        {
                            command.Parameters.AddWithValue("@JourneyId", stopover.JourneyID);
                            command.Parameters.AddWithValue("@Start", stopover.Start);
                            command.Parameters.AddWithValue("@End", stopover.End);
                            command.Parameters.AddWithValue("@VehicleType", (int)stopover.VehicleType);
                            command.Parameters.AddWithValue("@Distance", stopover.Distance);
                            command.Parameters.AddWithValue("@Emission", stopover.Emission);
                            newStopoverId = (int)await command.ExecuteScalarAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine(stopover.VehicleType);
                        throw;
                    }
                }
                string vehicleTypeName = stopover.VehicleType.GetDisplayName();
                return Json(new { success = true, stopoverId = newStopoverId, stopover = stopover, vehicleTypeName });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Stopover stopover)
        {
            stopover.CalculateEmission();

            try
            {
                using (SqlConnection conn = new(_connectionString))
                {
                    using (SqlCommand cmd = new("INSERT INTO Stopovers (Vehicle_ID, JourneyID, Distance, Start, [End], Emission) VALUES (@Vehicle_ID, @JourneyID, @Distance, @Start, @End, @Emission)", conn))
                    {
                        cmd.Parameters.Add("@Vehicle_ID", SqlDbType.Int).Value = stopover.VehicleType;
                        cmd.Parameters.Add("@JourneyID", SqlDbType.Int).Value = stopover.JourneyID;
                        cmd.Parameters.Add("@Distance", SqlDbType.Int).Value = stopover.Distance;
                        cmd.Parameters.Add("@Start", SqlDbType.VarChar).Value = stopover.Start;
                        cmd.Parameters.Add("@End", SqlDbType.VarChar).Value = stopover.End;
                        cmd.Parameters.Add("@Emission", SqlDbType.Float).Value = stopover.Emission;

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok();
        }



        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
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
            return Json(stopovers);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Stopover stopover;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Stopovers WHERE Stopover_ID = @Stopoverid ", conn))
                {
                    cmd.Parameters.AddWithValue("@stopoverid", id);

                    conn.Open();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                        {
                            return NotFound();
                        }
                        stopover = new Stopover
                        {
                            Stopover_ID = reader.GetInt32(0),
                            JourneyID = reader.GetInt32(2),
                            Distance = reader.GetInt32(3),
                            Start = reader.GetString(4),
                            End = reader.GetString(5),
                            Emission = reader.GetInt32(6)
                        };


                    }
                }
            }
            return View(stopover);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Stopover stopover, int journeyID)
        {
            Console.WriteLine("id: " + stopover.Stopover_ID);
            Console.WriteLine("Vehicle: " + stopover.VehicleType);
            
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Stopovers SET Vehicle_ID = @Vehicle_ID, Distance = @Distance, Start = @Start, [End] = @End, Emission = @Emission WHERE Stopover_ID = @Stopover_ID", conn))
                    {
                        cmd.Parameters.Add("@Vehicle_ID", SqlDbType.Int).Value = stopover.VehicleType;
                        cmd.Parameters.Add("@Distance", SqlDbType.Int).Value = stopover.Distance;
                        cmd.Parameters.Add("@Start", SqlDbType.VarChar).Value = stopover.Start;
                        cmd.Parameters.Add("@End", SqlDbType.VarChar).Value = stopover.End;
                        cmd.Parameters.Add("@Emission", SqlDbType.Float).Value = stopover.Emission;
                        cmd.Parameters.Add("@Stopover_ID", SqlDbType.Int).Value = stopover.Stopover_ID;

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return Redirect("/journey/details/" + stopover.JourneyID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int stopoverid)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("DELETE FROM Stopovers WHERE Stopover_ID = @StopoverID", conn))
                {
                    cmd.Parameters.AddWithValue("@StopoverID", stopoverid);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
