using CIS317_FinalProject.UI;
using CIS317_FinalProject.Database;

public class MainProgram
{
    static void Main(string[] args)
    {
        DatabaseHelpers.DatabaseInit("Database/DBScript.sql");

        Interfaces.MainScreen();
    }
}