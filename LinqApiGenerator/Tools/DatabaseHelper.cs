using System.Collections.Generic;
using System.Data.SqlClient;

public class DatabaseHelper
{
    public static List<string> GetTables(string connectionString)
    {
        List<string> tables = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }
        }
        return tables;
    }
}
