using CGI.Models;
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

        public ActionResult Stopovers(int journeyId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT j.*, s.* " +
                                                      "FROM Journeys j " +
                                                      "LEFT JOIN Stopovers s ON j.JourneyID = s.JourneyID " +
                                                      "WHERE j.JourneyID = @journeyId", connection);
                command.Parameters.AddWithValue("@journeyId", journeyId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Journey journey = null;
                List<Stopover> stopovers = new List<Stopover>();
                while (reader.Read())
                {
                    if (journey == null)
                    {
                        journey = new Journey
                        {
                            JourneyID = (int)reader["Journey_ID"],
                            UserID = (int)reader["User_ID"],
                            TotalDistance = (int)reader["Total_Distance"],
                            TotalEmission = (int)reader["Total_Emission"],
                            Start = (string)reader["Start"],
                            End = (string)reader["End"],
                            Date = ((DateTime)reader["Date"]).Date,
                            stopovers = new List<Stopover>()
                        };
                    }

                    Stopover stopover = new Stopover
                    {
                        StopoverID = (int)reader["Stopover_ID"],
                        VehicleID = (int)reader["Vehicle_ID"],
                        JourneyID = (int)reader["Journey_ID"],
                        Distance = (int)reader["Distance"],
                        Start = (string)reader["Start"],
                        End = (string)reader["End"],
                        Emission = (int)reader["Emission"]
                    };

                    journey.stopovers.Add(stopover);
                }

                reader.Close();

                return View(journey);
            }
        }

        [HttpGet]
        public IActionResult JourneyStopovers(int journeyID)
        {
            List<Stopover> stopovers = new List<Stopover>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Stopovers WHERE Journey_ID = @journeyID ORDER BY Stopover_Number ASC";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@journeyID", journeyID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Stopover stopover = new Stopover
                        {
                            StopoverID = (int)reader["Stopover_ID"],
                            JourneyID = (int)reader["Journey_ID"],
                            VehicleID = (int)reader["Vehicle_ID"],
                            Distance = (int)reader["Distance"],
                            Start = (string)reader["Start"],
                            End = (string)reader["End"],
                            Emission = (int)reader["Emission"]
                        };
                        stopovers.Add(stopover);
                    }
                    reader.Close();
                }
            }

            Journey journey = GetJourneyById(journeyID);
            journey.stopovers = stopovers;
            return View(journey);
        }

        private Journey GetJourneyById(int journeyID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "SELECT j.*, s.* FROM Journeys j LEFT JOIN Stopovers s ON j.JourneyID = s.JourneyID WHERE j.Journey_ID = @journeyID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@journeyID", journeyID);

                    SqlDataReader reader = command.ExecuteReader();

                    Journey journey = null;
                    while (reader.Read())
                    {
                        if (journey == null)
                        {
                            journey = new Journey
                            {
                                JourneyID = (int)reader["Journey_ID"],
                                UserID = (int)reader["User_ID"],
                                TotalDistance = (int)reader["Total_Distance"],
                                TotalEmission = (int)reader["Total_Emission"],
                                Start = (string)reader["Start"],
                                End = (string)reader["End"],
                                Date = ((DateTime)reader["Date"]).Date,
                                stopovers = new List<Stopover>()
                            };
                        }

                        if (reader["StopoverID"] != DBNull.Value)
                        {
                            Stopover stopover = new Stopover
                            {
                                StopoverID = Convert.ToInt32(reader["StopoverID"]),
                                VehicleID = Convert.ToInt32(reader["VehicleID"]),
                                JourneyID = Convert.ToInt32(reader["JourneyID"]),
                                Distance = Convert.ToInt32(reader["Distance"]),
                                Start = reader["Start"].ToString(),
                                End = reader["End"].ToString(),
                                Emission = Convert.ToInt32(reader["Emission"])
                            };

                            journey.stopovers.Add(stopover);
                        }
                    }
                    reader.Close();

                    return journey;
                }
            }
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
        public IActionResult DeleteConfirmed([FromBody] Journey journey)
        {
            int journeyId = data.Value<int>("journeyId");

            // Delete an existing journey from the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Journeys WHERE Journey_ID = @JourneyID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@JourneyID", journeyId);

                    command.ExecuteNonQuery();
                }
            }

            // Return a successful response
            return Ok();
        }
    }
}
