using Spectre.Console;
using System.Globalization;

namespace CodingTracker
{
    internal class Helpers
    {
        CultureInfo enUS = new CultureInfo("en-US");

        internal string CheckDateTime()
        {
            string date = "";
            bool isValid = false;

            do
            {
                date = AnsiConsole.Ask<string>("Format is [green]dd-MM-yy HH:mm[/] or type t for today's date and time: ");
                date = date.Trim().ToLower();

                if (date == "t")
                    date = DateTime.Now.ToString("dd-MM-yy HH:mm");

                if (date != "0")
                    isValid = DateTime.TryParseExact(date, "dd-MM-yy HH:mm", enUS, DateTimeStyles.None, out _);
                else
                    isValid = true;

                if (!isValid)
                {
                    AnsiConsole.Markup("[red]Please input the right date format![/]\n");
                    Console.ReadLine();
                }

            } while (!isValid);

            return date;
        }

        internal TimeSpan GetDuration(string startDateTime, string endDateTime)
        {
            DateTime start = DateTime.ParseExact(startDateTime, "dd-MM-yy HH:mm", null);
            DateTime end = DateTime.ParseExact(endDateTime, "dd-MM-yy HH:mm", null);
            
            TimeSpan duration = end - start;

            return duration;
        }
       
    }
}
