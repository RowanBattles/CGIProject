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
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Stopovers (VehicleID, JourneyID, Distance, Start, End, Emission) VALUES (@VehicleID, @JourneyID, @Distance, @Start, @End, @Emission)", conn))
                    {
                        cmd.Parameters.AddWithValue("@VehicleID", stopover.VehicleType);
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
        public async Task<IActionResult> Edit(Stopover stopover)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new("UPDATE Stopovers SET Vehicle_ID = @Vehicle_ID, Distance = @Distance, Start = @Start, [End] = @End, Emission = @Emission WHERE Stopover_ID = @Stopover_ID", conn))
                {

                    cmd.Parameters.Add("@Vehicle_ID", SqlDbType.Int).Value = stopover.VehicleType;
                    cmd.Parameters.Add("@Distance", SqlDbType.Int).Value = stopover.Distance;
                    cmd.Parameters.Add("@Start", SqlDbType.VarChar).Value = stopover.Start;
                    cmd.Parameters.Add("@End", SqlDbType.VarChar).Value = stopover.End;
                    cmd.Parameters.Add("@Emission", SqlDbType.Float).Value = stopover.Emission;
                    cmd.Parameters.Add("@Stopover_ID", SqlDbType.Int).Value = stopover.Stopover_ID;

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Redirect("/journey?id=" + stopover.JourneyID);
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
