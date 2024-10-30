using System.Data.SQLite;

namespace CIS317_FinalProject.Database;

public class DatabaseHelpers
{
    private static readonly string ConnectionString = "Data Source=Data/Project.db;BusyTimeout=5000";

    public static SQLiteConnection CreateDatabaseConnection()
    {
        SQLiteConnection conn = new SQLiteConnection(ConnectionString);
        conn.Open();

        return conn;
    }

    public static void DatabaseInit(string path)
    {
        using (SQLiteConnection conn = CreateDatabaseConnection())
        {
            SQLiteCommand cmd = new SQLiteCommand(File.ReadAllText(path), conn);
            cmd.ExecuteNonQuery();
        }
    }
}