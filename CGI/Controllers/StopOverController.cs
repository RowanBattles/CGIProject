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

        [HttpGet]
        public IActionResult CalculateEmission(Vehicle_ID vehicleType, int distance)
        {
            Stopover stopover = new Stopover
            {
                VehicleType = vehicleType,
                Distance = distance
            };
            stopover.CalculateEmission();
            return Json(new { emission = stopover.Emission });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitStopovers(string stopoversJson)
        {
            List<Stopover> stopovers = JsonConvert.DeserializeObject<List<Stopover>>(stopoversJson);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                foreach (var stopover in stopovers)
                {

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Stopovers (Vehicle_ID, Journey_ID, Distance, Start, End, Emission) VALUES (@VehicleID, @JourneyID, @Distance, @Start, @End, @Emission)", conn))

                    {
                        cmd.Parameters.AddWithValue("@VehicleID", (int)stopover.VehicleType);
                        cmd.Parameters.AddWithValue("@JourneyID", stopover.JourneyID);
                        cmd.Parameters.AddWithValue("@Distance", stopover.Distance);
                        cmd.Parameters.AddWithValue("@Start", stopover.Start);
                        cmd.Parameters.AddWithValue("@End", stopover.End);
                        cmd.Parameters.AddWithValue("@Emission", stopover.Emission);

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        conn.Close();
                    }
                }
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand stopoverCmd = new SqlCommand("UPDATE Stopovers SET Vehicle_ID = @Vehicle_ID, Distance = @Distance, Start = @Start, [End] = @End, Emission = @Emission WHERE Stopover_ID = @Stopover_ID", conn))
                {
                    stopoverCmd.Parameters.AddWithValue("@Vehicle_ID", (int)stopover.VehicleType);
                    stopoverCmd.Parameters.AddWithValue("@Distance", stopover.Distance);
                    stopoverCmd.Parameters.AddWithValue("@Start", stopover.Start);
                    stopoverCmd.Parameters.AddWithValue("@End", stopover.End);
                    stopoverCmd.Parameters.AddWithValue("@Emission", stopover.Emission);
                    stopoverCmd.Parameters.AddWithValue("@Stopover_ID", stopover.Stopover_ID);

                    await stopoverCmd.ExecuteNonQueryAsync();
                }

                using (SqlCommand journeyCmd = new SqlCommand("UPDATE Journeys SET Start = @Start, [End] = @End, Total_Distance = @Total_Distance, Total_Emission = @Total_Emission WHERE Journey_ID = @Journey_ID", conn))
                {
                    using (SqlCommand startCmd = new SqlCommand("SELECT TOP 1 Start FROM Stopovers WHERE Journey_ID = @Journey_ID ORDER BY Stopover_ID ASC", conn))
                    {
                        startCmd.Parameters.AddWithValue("@Journey_ID", journeyID);
                        string journeyStart = (string)await startCmd.ExecuteScalarAsync();
                        journeyCmd.Parameters.AddWithValue("@Start", journeyStart);
                    }

                    using (SqlCommand endCmd = new SqlCommand("SELECT TOP 1 [End] FROM Stopovers WHERE Journey_ID = @Journey_ID ORDER BY Stopover_ID DESC", conn))
                    {
                        endCmd.Parameters.AddWithValue("@Journey_ID", journeyID);
                        string journeyEnd = (string)await endCmd.ExecuteScalarAsync();
                        journeyCmd.Parameters.AddWithValue("@End", journeyEnd);
                    }

                    using (SqlCommand distanceCmd = new SqlCommand("SELECT SUM(Distance) FROM Stopovers WHERE Journey_ID = @Journey_ID", conn))
                    {
                        distanceCmd.Parameters.AddWithValue("@Journey_ID", journeyID);
                        int totalDistance = (int)await distanceCmd.ExecuteScalarAsync();
                        journeyCmd.Parameters.AddWithValue("@Total_Distance", totalDistance);
                    }

                    using (SqlCommand emissionCmd = new SqlCommand("SELECT SUM(Emission) FROM Stopovers WHERE Journey_ID = @Journey_ID", conn))
                    {
                        emissionCmd.Parameters.AddWithValue("@Journey_ID", journeyID);
                        int totalEmission = (int)await emissionCmd.ExecuteScalarAsync();
                        journeyCmd.Parameters.AddWithValue("@Total_Emission", totalEmission);
                    }

                    journeyCmd.Parameters.AddWithValue("@Journey_ID", journeyID);

                    await journeyCmd.ExecuteNonQueryAsync();
                }
            }

            return Redirect("/journey/details/" + stopover.JourneyID);
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
