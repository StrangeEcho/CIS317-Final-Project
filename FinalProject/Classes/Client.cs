namespace FinalProject.Classes 
{
    public class Client 
    {
        public int ClientID { get; private set; }
        public string? Name { get; private set; }
        public string? Address { get; private set; }
        public string? Email { get; private set; }
        public double? Balance { get; private set; }

        public Client(int clientid) 
        {
            ClientID = clientid;
            // Query the database for the rest of the information
        }

        public void UpdateBalance(double newBalance) 
        {
            Balance = newBalance;

            //Update in database
        }
    }

}