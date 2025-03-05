using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

public class DatabaseValidator
{
    private static readonly Regex connectionStringRegex = new Regex(
        @"^Server=.*;Database=.*;User Id=.*;Password=.*;$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool ValidateConnectionStringFormat(string connectionString)
    {
        return connectionStringRegex.IsMatch(connectionString);
    }

    public static async Task<bool> CanConnectToDatabaseAsync(string connectionString, int timeoutSeconds = 1)
    {
        if (string.IsNullOrEmpty(connectionString)) return false;

        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(cts.Token);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
