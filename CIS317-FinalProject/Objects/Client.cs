using System.Data.SQLite;
using CIS317_FinalProject.Database;

namespace CIS317_FinalProject.Objects;

public class Client
{
    public int? Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public double Balance { get; set; }

    public Client(int id, string username, string name, string email, double balance)
    {
        Id = id;
        Username = username;
        Name = name;
        Email = email;
        Balance = balance;
    }

    public Client(string username, string name, string email, double balance)
    {
        Username = username;
        Name = name;
        Email = email;
        Balance = balance;
    }

    public static Client? GetClientById(int id)
    {
        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText = "SELECT * FROM Clients WHERE ClientId = @Id";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Client(
                            id,
                            rdr.GetString(1),
                            rdr.GetString(2),
                            rdr.GetString(3),
                            rdr.GetDouble(4));
                    }
                }
            }
        }

        return null;
    }

    public void UpdateBalance()
    {
        Console.WriteLine($"==== Enter the new balance for {Username} ====");
        Console.Write("New Balance (Negatives Accepted): $");
        string? bal = Console.ReadLine();

        Balance = Convert.ToDouble(bal) + Balance; // Update Object

        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText = "UPDATE Clients SET Balance = @Balance WHERE ClientId = @ClientId";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                cmd.Parameters.AddWithValue("@Balance", Balance);
                cmd.Parameters.AddWithValue("@ClientId", Id);
                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine($"Balance for Client {Id} Updated!");
    }

    public void UpdateInformation()
    {
        Console.WriteLine($"==== Updating Information for {Username} ====\n\n");

        Console.Write("Enter New Username: ");
        string? user = Console.ReadLine();
        Console.Write("Enter New Name: ");
        string? name = Console.ReadLine();
        Console.Write("Enter New Email: ");
        string? email = Console.ReadLine();

        Username = user;
        Name = name;
        Email = email;

        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText =
                "UPDATE Clients SET Username = @Username, Name = @Name, Email = @Email WHERE ClientId = @ClientId";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@ClientId", Id);
                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine($"Client at ID {Id} Updated.");
    }

    public void InsertNew()
    {
        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText =
                "INSERT INTO Clients (Username, Name, Email, Balance) VALUES (@Username, @Name, @Email, @Balance)";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Balance", Balance);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Delete()
    {
        Console.Write($"You are choosing to delete client record for {Username} at ID: {Id}. Are you sure (yes/no)?: ");
        string? choice = Console.ReadLine();

        if (choice == "yes")
        {
            using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
            {
                string cmdText = "DELETE FROM Clients WHERE ClientId = @ClientId";
                using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientId", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}