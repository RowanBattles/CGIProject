using CGI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace CGI.Controllers
{
    public class GroupController : Controller
    {
        private readonly string _connectionString;

        public GroupController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Group> groups = new List<Group>();
            var loggedInUserId = await GetLoggedInUserId();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sqlSelectGroups = "SELECT G.*, " +
                                         "      CAST(CASE WHEN GU.group_id IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS user_is_in_group " +
                                         "FROM Groups G " +
                                         "LEFT JOIN GroupUsers GU " +
                                         "ON G.id = GU.group_id AND GU.user_id = @userId";


                using (SqlCommand cmd = new SqlCommand(sqlSelectGroups, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var group = new Group();

                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        {
                            group.Id = (int)reader["id"];
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        {
                            group.Name = (string)reader["name"];
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("user_is_in_group")))
                        {
                            group.UserIsInGroup = (bool)reader["user_is_in_group"];
                        }

                        groups.Add(group);
                    }


                    reader.Close();
                }
            }

            string id = Request.Query["id"];

            if (string.IsNullOrEmpty(id))
            {
                id = "0";
            }

            GroupViewModel model = new()
            {
                Id = int.Parse(id),
                Groups = groups,
            };


            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            List<Group> groups = new List<Group>();
            var leaderboardViewModels = new List<LeaderboardViewModel>();
            var userIsPartOfGroup = false;
            var loggedInUserIsGroupAdmin = false;

            var loggedInUserId = await GetLoggedInUserId();


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sqlSelectGroups = "SELECT G.*, " +
                                         "      CAST(CASE WHEN GU.group_id IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS user_is_in_group " +
                                         "FROM Groups G " +
                                         "LEFT JOIN GroupUsers GU " +
                                         "ON G.id = GU.group_id AND GU.user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(sqlSelectGroups, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", loggedInUserId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var group = new Group();

                        if (!reader.IsDBNull(reader.GetOrdinal("id")))
                        {
                            group.Id = (int)reader["id"];
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("name")))
                        {
                            group.Name = (string)reader["name"];
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("user_is_in_group")))
                        {
                            group.UserIsInGroup = (bool)reader["user_is_in_group"];
                        }

                        groups.Add(group);
                    }


                    reader.Close();
                }


                string sqlSelectUsers =
                    "SELECT U.User_ID, U.FullName, U.Score, GU.user_is_admin " +
                    "FROM Users U " +
                    "INNER JOIN GroupUsers GU ON U.User_ID = GU.User_ID " +
                    "WHERE GU.Group_ID = @groupId " +
                    "GROUP BY U.User_ID, U.FullName, U.Score, GU.user_is_admin " +
                    "ORDER BY U.Score DESC;";


                await using (var cmd = new SqlCommand(sqlSelectUsers, conn))
                {
                    cmd.Parameters.AddWithValue("@groupId", id);

                    var reader = await cmd.ExecuteReaderAsync();


                    while (reader.Read())
                    {
                        var leaderboardViewModel = new LeaderboardViewModel();

                        if (!reader.IsDBNull(reader.GetOrdinal("User_ID")))
                        {
                            var userId = (int)reader["User_ID"];
                            leaderboardViewModel.Id = userId;
                            if (userId == loggedInUserId)
                            {
                                userIsPartOfGroup = true;
                            }
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("Fullname")))
                        {
                            leaderboardViewModel.userName = (string)reader["Fullname"];
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("Score")))
                        {
                            leaderboardViewModel.score = (int)reader["Score"];
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("user_is_admin")))
                        {
                            var isAdmin = (bool)reader["user_is_admin"];
                            leaderboardViewModel.UserIsGroupAdmin = isAdmin;

                            if (isAdmin && leaderboardViewModel.Id == loggedInUserId)
                            {
                                loggedInUserIsGroupAdmin = true;
                            }
                        }


                        leaderboardViewModels.Add(leaderboardViewModel);
                    }

                    reader.Close();
                }
            }

            GroupViewModel model = new()
            {
                Id = id,
                UserIsGroupAdmin = loggedInUserIsGroupAdmin,
                UserIsPartOfGroup = userIsPartOfGroup,
                Groups = groups,
                Leaderboards = leaderboardViewModels
            };


            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(int userId, int groupId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                           "DELETE FROM GroupUsers " +
                           "WHERE group_id = @groupId " +
                           "AND user_id = @userId;"
                           , conn)
                      )
                {
                    cmd.Parameters.AddWithValue("@groupId", groupId);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    conn.Open();
                    await cmd.ExecuteScalarAsync();
                    conn.Close();

                    return RedirectToAction("Details", new { id = groupId });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Join(int groupId)
        {
            var loggedInUserId = await GetLoggedInUserId();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                           "INSERT INTO GroupUsers (group_id, user_id) " +
                           "VALUES (@groupId, @userId)", conn)
                      )
                {
                    cmd.Parameters.AddWithValue("@groupId", groupId);
                    cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                    // Execute the command here
                    // cmd.ExecuteNonQuery();

                    conn.Open();
                    await cmd.ExecuteScalarAsync();
                    conn.Close();

                    return RedirectToAction("Details", new { id = groupId });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int groupId)
        {
            var loggedInUserId = await GetLoggedInUserId();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                           "DELETE FROM GroupUsers " +
                           "WHERE group_id = @groupId " +
                           "AND user_id = @userId " +
                           "AND user_is_admin = 0;"
                           , conn)
                      )
                {
                    cmd.Parameters.AddWithValue("@groupId", groupId);
                    cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                    conn.Open();
                    await cmd.ExecuteScalarAsync();
                    conn.Close();

                    return RedirectToAction("Details", new { id = groupId });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(string groupName)
        {
            var loggedInUserId = await GetLoggedInUserId();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                           "INSERT INTO GROUPS (name) " +
                           "VALUES (@groupName);" +
                           "" +
                           "DECLARE @group_id INT;" +
                           "SET @group_id = SCOPE_IDENTITY();" +
                           "" +
                           "INSERT INTO GroupUsers (group_id, user_id, user_is_admin)" +
                           "VALUES (@group_id, @userId, 1);" +
                           "" +
                           "SELECT @group_id;"
                           , conn)
                      )
                {
                    cmd.Parameters.AddWithValue("@groupName", groupName);
                    cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                    conn.Open();
                    int groupId = (int)(await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException());
                    conn.Close();

                    return RedirectToAction("Details", new { id = groupId });
                }
            }
        }


        public async Task<int?> GetLoggedInUserId()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var userInfo = new AccountController(null).GetAuth0UserInfo(idToken);
            string userId = userInfo.UserId;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT User_ID, FullName FROM Users WHERE UUID = @UserId",
                           conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                int userIdFromDb = reader.GetInt32(0);
                                return userIdFromDb;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}