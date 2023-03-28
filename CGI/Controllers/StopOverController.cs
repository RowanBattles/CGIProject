using CGI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CGI.Controllers
{
    public class StopOverController : Controller
    {
        private readonly string _connectionString;
        public IActionResult Index()
        {
            return View();
        }
        public StopOverController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IActionResult> Edit(Stopover stopover)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Stopovers SET StopoverID = @StopoverID, VehicleID = @VehicleID, JourneyID = @JourneyID, Distance = @Distance, Start = @Start, End = @End, Emission = @Emission WHERE StopoverID = @StopoverID", conn))
                {
                    cmd.Parameters.AddWithValue("@StopoverID", stopover.StopoverID);
                    cmd.Parameters.AddWithValue("@VehicleID", stopover.VehicleID);
                    cmd.Parameters.AddWithValue("@JourneyID", stopover.JourneyID);
                    cmd.Parameters.AddWithValue("@Distance", stopover.Distance);
                    cmd.Parameters.AddWithValue("@Start", stopover.Start);
                    cmd.Parameters.AddWithValue("@End", stopover.End);
                    cmd.Parameters.AddWithValue("@Emission", stopover.Emission);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int stopoverid)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Stopovers WHERE Stopover_ID = @StopoverID", conn))
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
