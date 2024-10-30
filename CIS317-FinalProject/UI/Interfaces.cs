using System.Data.SQLite;
using CIS317_FinalProject.Database;
using CIS317_FinalProject.Utils;
using CIS317_FinalProject.Objects;

namespace CIS317_FinalProject.UI;

public static class Interfaces
{
    public static void MainScreen()
    {
        Console.Clear();
        Console.WriteLine("++++ CIS317 Final Project Banker System ++++\n\n");
        Console.Write("Press Enter to proceed to Banker Login or type EXIT to Exit Program: ");

        string? choice = Console.ReadLine();

        if (string.IsNullOrEmpty(choice))
        {
            BankerLogin();
        }
        else if (choice.ToLower() == "exit")
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid Choice");
        }
    }

    public static void BankerLogin()
    {
        Console.Clear();
        Console.WriteLine("++++ Welcome to Banker Login ++++\n\n");

        for (int i = 0; i < 3; i++)
        {
            Console.Write("Enter your username: ");
            string? user = Console.ReadLine();
            Console.Write("Enter your password: ");
            string? pass = Console.ReadLine();

            if (Validators.BankerLogin(user, pass))
            {
                BankerMainScreen(user);
                break;
            }
            else
            {
                Console.WriteLine($"Incorrect Login... Try again. (Attempt {i + 1}/3)");

                if (i == 2)
                {
                    Console.WriteLine("TOO MANY LOGIN ATTEMPTS. Going Back to Main Screen");
                    MainScreen();
                    break;
                }
            }
        }
    }

    public static void BankerMainScreen(string? bankerUserName)
    {
        Console.Clear();
        Console.WriteLine($"++++ Welcome {bankerUserName} to the Banker System ++++\n\n");

        Console.WriteLine(
            "(1). Add New Client | (2). Manage Existing Client | (3). View Existing Clients | (4). Main Screen");
        while (true)
        {
            Console.Write("Enter choice (1-4): ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ClientBuilder();
                    break;
                case "2":
                    ClientLookup(bankerUserName);
                    break;
                case "3":
                    ExistingClientScreen(bankerUserName);
                    break;
                case "4":
                    MainScreen();
                    break;
            }
            BankerMainScreen(bankerUserName);
        }
    }

    public static void ClientBuilder()
    {
        Console.Clear();
        Console.WriteLine("++++ Client Builder Interface ++++\n\n");

        Console.Write("Enter Client Username: ");
        string? user = Console.ReadLine();
        Console.Write("Enter Client Name: ");
        string? name = Console.ReadLine();
        Console.Write("Enter Client Email: ");
        string? email = Console.ReadLine();
        Console.Write("Enter Client Starting Balance: $");
        string? bal = Console.ReadLine();

        Client client = new Client(user, name, email, Convert.ToDouble(bal));
        client.InsertNew();
    }

    public static void ClientLookup(string bankerUsername)
    {
        Console.Clear();
        Console.WriteLine($"++++ Client Lookup via **{bankerUsername}** ++++\n\n");
        while (true)
        {
            Console.Write("Enter ID (Enter EXIT to go back to Banker Main Screen): ");
            string? inp = Console.ReadLine();
            if (inp.ToLower() == "exit")
            {
                BankerMainScreen(bankerUsername);
            }

            if (int.TryParse(inp, out int id))
            {
                if (Validators.ClientExistence(id))
                {
                    ClientManagementScreen(id, bankerUsername);
                    break;
                }
                else
                {
                    Console.WriteLine($"No Client found matching ID: {id}\n");
                }
            }
            else
            {
                Console.WriteLine("Bad input type. Make sure you enter an integer...\n");
            }
        }
    }

    public static void ClientManagementScreen(int id, string bankerUsername)
    {
        Console.Clear();
        Client? client = Client.GetClientById(id);

        if (client == null)
        {
            Console.WriteLine("Client not found. Returning to Main Screen.");
            MainScreen();
            return;
        }

        Console.WriteLine($"++++ Now managing **{client.Username}** at ID: **{id}** via **{bankerUsername}** ++++\n\n");
        Console.WriteLine(
            $"Client ID: {id}\nClient Name: {client.Name}\nClient Username: {client.Username}\nClient Email: {client.Email}\nClient Balance: ${client.Balance}");
        Console.WriteLine(
            "\n\n(1). Update Balance | (2). Manage Client Information | (3). Delete Client | (4). Banker Screen | (5). Main Screen");
        while (true)
        {
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    client.UpdateBalance();
                    break;
                case "2":
                    client.UpdateInformation();
                    break;
                case "3":
                    client.Delete();
                    BankerMainScreen(bankerUsername);
                    break;
                case "4":
                    BankerMainScreen(bankerUsername);
                    break;
                case "5":
                    MainScreen();
                    break;
            }
            ClientManagementScreen(id, bankerUsername);
        }
    }

    public static void ExistingClientScreen(string bankerUsername)
    {
        List<Client> clientArray = new List<Client>();

        using (SQLiteConnection conn = DatabaseHelpers.CreateDatabaseConnection())
        {
            string cmdText = "SELECT * FROM Clients";
            using (SQLiteCommand cmd = new SQLiteCommand(cmdText, conn))
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    clientArray.Add(new Client(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3),
                        rdr.GetDouble(4)));
                }
            }
        }

        Console.Clear();
        Console.WriteLine($"++++ Existing Clients ( Showing 3 / {clientArray.Count} )++++\n\n");

        for (int i = 0; i <= (clientArray.Count < 3 ? clientArray.Count : 2); i++)
        {
            Console.WriteLine(
                $"ID: {clientArray[i].Id}\nUsername: {clientArray[i].Username}\nName: {clientArray[i].Name}\nEmail: {clientArray[i].Email}\nBalance: {clientArray[i].Balance}\n\n");
        }

        if (clientArray.Count > 3)
        {
            Console.WriteLine("== Truncated... Too much depth ==");
        }

        Console.Write("Press Enter to exit client list: ");
        Console.ReadLine();
    }
}