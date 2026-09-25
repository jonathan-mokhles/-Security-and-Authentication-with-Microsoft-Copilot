using System;
using System.Data.SqlClient;
using BCrypt.Net;

public class AuthService
{
    private readonly string _connectionString;

    public AuthService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool RegisterUser(string username, string email, string password, string role)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        using (var connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES (@Username, @Email, @PasswordHash, @Role)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.Parameters.AddWithValue("@Role", role);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool AuthenticateUser(string username, string password)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                var result = command.ExecuteScalar();
                if (result == null) return false;

                string storedHash = result.ToString();
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
        }
    }
    public bool AuthorizeUser(string username, string requiredRole)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT Role FROM Users WHERE Username = @Username";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Username", username);
            connection.Open();

            var role = command.ExecuteScalar()?.ToString();
            return role == requiredRole;
        }
    }
}

}
