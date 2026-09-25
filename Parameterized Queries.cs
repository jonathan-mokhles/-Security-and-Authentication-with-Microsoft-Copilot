using (var connection = new SqlConnection(connectionString))
{
    string query = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email)";
    using (var command = new SqlCommand(query, connection))
    {
        command.Parameters.AddWithValue("@Username", InputSanitizer.SanitizeUsername(username));
        command.Parameters.AddWithValue("@Email", InputSanitizer.SanitizeEmail(email));

        connection.Open();
        command.ExecuteNonQuery();
    }
}
