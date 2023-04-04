using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CGI.Controllers;

public class AccountController : Controller
{
    private readonly string _connectionString;
    public AccountController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public (string UserId, string Name) GetAuth0UserInfo(string idToken)

    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(idToken);
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        return (userId, name);
    }


    private async Task InsertUserIdIntoDatabase(string userId, string name)
    {

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Check if the user_id already exists
            await using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE UUID = @userId", connection))
            {
                checkCommand.Parameters.AddWithValue("@userId", userId);
                
                int userCount = (int)await checkCommand.ExecuteScalarAsync();
                if (userCount > 0)
                {
                    // User already exists, so return without inserting
                    return;
                }
            }

            await using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Users (UUID, FullName) VALUES (@userId, @name)", connection))
            {
                insertCommand.Parameters.AddWithValue("@userId", userId);
                insertCommand.Parameters.AddWithValue("@name", name);
                await insertCommand.ExecuteNonQueryAsync();
            }


        }
    }

    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/Account/Callback" }, "Auth0");
    }

    public async Task<IActionResult> Callback()
    {
        // Get access token
        var idToken = await HttpContext.GetTokenAsync("id_token");


        // Get Name and ID of user
        var userInfo = GetAuth0UserInfo(idToken);
        string userId = userInfo.UserId;
        string userName = userInfo.Name;

        // Insert the user into the database if they don't already exist
        await InsertUserIdIntoDatabase(userId, userName);

        // Redirect to a different view after a successful login
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task Logout()
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(Url.Action("Index", "Home"))
            .Build();

        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }


}