# Coding Tracker
Coding Tracker is the third green belt project from the C# Academy. It is similar to the Habit Logger on having CRUD functions but the difference is the app asks for the user's beginning coding date time and end coding date time to find the coding time duration. This is also an opportunity to learn Object-Oriented-Programming (OOP). I programmed using C#, SQlite, Dapper and Spectre.Console with Visual Studio 2026.

## Requirements
- Same requirements as the habit logger in terms of database and CRUD.
- Have separate classes in different files.
- Create a configuration file called appsettings.json which contains the database path and connection strings.
- User should put their start time and end time in specific format and not their duration. Duration should be calculated based on the start time and end time. 
- Data need to be shown through Spectre.Console library
- Need to use Dapper ORM for data access instead of ADO.NET.

## Features
- Console based UI using the Spectre.Console library
![Image](/Assets/1.png)
- CRUD functions:
    - Users insert their start date time and end date time in format dd/MM/yy HH:mm
    - Users can read, update and delete their coding sessions by inputting the session id.
    - Dates and numbers are validated to check if they're in the right format and check if the end date submission has to be after the start date.
![Image](/Assets/2.png)

## Challenges
- Understanding what appsettings.json. This is the first hurdle of trying to understand what this script does. From my understanding it's a configuration file that stores data like the database connection string which is more secure than hardcoding it in the C# script. This was hardcoded in the habit log project. 
- Learning Dapper ORM. I thought it would be another long code I had to learn, but it actually condensed the code from giving large command text to small sql strings to use Dapper's execute and Query<T>(). It also teaches me to use @Parameters from SQL injection attacks and anonymous object to bridge the SQL parameters with the local paremeters.  
- Spectre.Console is not that challenging to learn as they provide a very clear documentation on how to use them. The hard part is how to use them to separate the data and visuals in different classes from each other.
- Calculating the duration between two date times. I had to use 24 hour time to calculate the duration as it doesn't require to convert for calculations. The calculations is just simply subtract the end date with the start date with TimeSpan as a data type to represent total hours and minutes of the duration.

## References
- https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json
- https://stackoverflow.com/questions/39157781/the-configuration-file-appsettings-json-was-not-found-and-is-not-optional
- https://www.learndapper.com/non-query#dapper-execute
- https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-10.0
- https://spectreconsole.net/console