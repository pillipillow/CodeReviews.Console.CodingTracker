using Spectre.Console;
using CodingTracker.Models;

namespace CodingTracker
{
    internal class UserInterface
    {
        DatabaseManager databaseManager = new DatabaseManager();
        Helpers helpers = new Helpers();

        internal void MainMenu()
        {
            bool isCloseApp = false;
            while (!isCloseApp) 
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold yellow]---Welcome to the Coding Tracker[/]---\n");

                var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<Enums.MenuOptions>()
                        .Title("What would you like to do?")
                        .AddChoices(Enum.GetValues<Enums.MenuOptions>()));

                switch (choice)
                {
                    case Enums.MenuOptions.ViewSessions:
                        ViewSessions();
                        break;
                    case Enums.MenuOptions.InsertSessions:
                        InsertSessions();
                        break;
                    case Enums.MenuOptions.UpdateSessions:
                        UpdateSessions();
                        break;
                    case Enums.MenuOptions.DeleteSessions:
                        DeleteSessions();
                        break;
                    case Enums.MenuOptions.CloseApp:
                        isCloseApp = true;   
                        break;

                }

            }
        }

        private void ViewSessions()
        {
            var sessions = databaseManager.Get();

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No sessions recorded.[/]\nPress enter key to go back to Main Menu.");
                Console.ReadLine();
                return;
            }

            DrawTable(sessions);

            AnsiConsole.MarkupLine("Press enter key to go back to Main Menu.");
            Console.ReadLine();
        }

        private void InsertSessions()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]---Insert Coding Sessions---[/]");
            
            AnsiConsole.MarkupLine("Please insert the [bold green]start[/] date and time (or type 0 to go back to the Main Menu).");
            var startDateTime = helpers.CheckDateTime();
            if (startDateTime == "0") return;

            AnsiConsole.MarkupLine("\nPlease insert the [bold green]end[/] date and time (or type 0 to go back to the Main Menu).");
            var endDateTime = helpers.CheckDateTime();
            if (endDateTime == "0") return;

            var duration = helpers.GetDuration(startDateTime, endDateTime);

            if (duration < TimeSpan.Zero)
            {
                AnsiConsole.Markup("[red]End date cannot be before start date! Please try again.[/]\n");
                Console.ReadLine();

            }
            else
            {
                CodingSession session = new CodingSession();
                session.StartTime = startDateTime;
                session.EndTime = endDateTime;
                session.Duration = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}";

                int rows = databaseManager.Post(session);

                if (rows > 0)
                {
                    AnsiConsole.MarkupLine("\n[green]Session added sucessfully![/]");
                    Console.ReadLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("\n[red]Session added failed! Try again later[/]");
                    Console.ReadLine();
                }

            }
            
        }

        private void UpdateSessions()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]---Update Coding Sessions---[/]");

            var sessions = databaseManager.Get();

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[red]No sessions recorded to update.[/]\nPress enter key to go back to Main Menu.");
                Console.ReadLine();
                return;
            }

            DrawTable(sessions);

            int inputID = AnsiConsole.Ask<int>("Please insert the number ID to update the session (or type 0 to go back to the Main Menu). ");

            if (inputID == 0) return;

            if (!databaseManager.CheckIdExists(inputID))
            {
                AnsiConsole.MarkupLine($"\n[red]Session with ID {inputID} doesn't exist.[/]");
                Console.ReadLine();
                UpdateSessions();
            }
            else
            {
                var sessionChoice = databaseManager.GetSessionByID(inputID);

                bool isUpdating = true;
                while (isUpdating)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[bold yellow]---Update Coding Sessions---[/]");
                    DrawTable(sessions);

                    var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title($"What would you like to do with Session {inputID}?")
                        .AddChoices("Update Start Time","Update End Time","Save and Update Database","Return to Main Menu"));

                    switch (choice)
                    {
                        case "Update Start Time":
                            AnsiConsole.MarkupLine("Please insert the [bold green]start[/] date and time.");
                            sessionChoice.StartTime = helpers.CheckDateTime();
                            break;
                        case "Update End Time":
                            AnsiConsole.MarkupLine("\nPlease insert the [bold green]end[/] date and time.");
                            sessionChoice.EndTime = helpers.CheckDateTime();
                            break;
                        case "Save and Update Database":
                            var duration = helpers.GetDuration(sessionChoice.StartTime, sessionChoice.EndTime);
                            if (duration < TimeSpan.Zero)
                            {
                                AnsiConsole.Markup("[red]End date cannot be before start date! Please try again.[/]\n");
                                Console.ReadLine();
                            }
                            else
                            {
                                sessionChoice.Duration = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}";
                                databaseManager.Update(sessionChoice);
                                AnsiConsole.Markup("[green]Session has been updated![/]\n");
                                Console.ReadLine();
                                isUpdating = false;
                            }
                            break;
                        case "Return to Main Menu":
                            isUpdating = false;
                            break;

                    }                
                }

            }
        }


        private void DeleteSessions()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]---Delete Coding Sessions---[/]");

            var sessions = databaseManager.Get();

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[red]No sessions recorded to delete.[/]\nPress enter key to go back to Main Menu.");
                Console.ReadLine();
                return;
            }

            DrawTable(sessions);

            int inputID = AnsiConsole.Ask<int>("Please insert the number ID to delete the session (or type 0 to go back to the Main Menu). ");

            if (inputID == 0) return;

            if (!databaseManager.CheckIdExists(inputID))
            {
                AnsiConsole.MarkupLine($"\n[red]Session with ID {inputID} doesn't exist.[/]");
                Console.ReadLine();
                DeleteSessions();
            }
            else
            {
                if (AnsiConsole.Confirm("Are you sure you want to delete this session?"))
                {
                    databaseManager.Delete(inputID);
                    AnsiConsole.MarkupLine($"\n[green]Session with ID {inputID} has been deleted![/]");
                    Console.ReadLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Session deletion cancelled[/]");
                    Console.ReadLine();
                    return;
                }     
            }
        }


        private void DrawTable(List<CodingSession> sessions)
        {
            var table = new Table();

            table.AddColumn("[bold]ID[/]");
            table.AddColumn("[bold]Start Date[/]");
            table.AddColumn("[bold]End Date[/]");
            table.AddColumn("[bold]Duration[/]");

            foreach (var session in sessions)
            {
                table.AddRow(session.Id.ToString(), $"{session.StartTime}", $"{session.EndTime}", $"{session.Duration}");
            }

            AnsiConsole.Write(table);
        }
        
    }
}
