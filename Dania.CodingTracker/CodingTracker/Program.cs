namespace CodingTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new DatabaseManager();
            UserInterface userInterface = new UserInterface();

            databaseManager.CreateTable();
            userInterface.MainMenu();
        }
    }
}
