using (var connection = new SqlConnection(connectionString))
{
    string query = "SELECT * FROM Users WHERE Username = @Username";
    using (var command = new SqlCommand(query, connection))
    {
        command.Parameters.AddWithValue("@Username", username);
        connection.Open();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                // Process user data safely
            }
        }
    }
}
