using System.Data.SQLite;
using CIS317_FinalProject.Database;

namespace CIS317_FinalProject.Utils;

public class Validators
{
    public static bool BankerLogin(string? username, string? password)
    {
        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText = "SELECT COUNT(*) FROM Bankers WHERE Username = @Username AND Password = @Password";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count >= 1;
            }
        }
    }

    public static bool ClientExistence(int id)
    {
        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText = "SELECT COUNT(*) FROM Clients WHERE ClientId = @Id";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count >= 1;
            }
        }
    }
}