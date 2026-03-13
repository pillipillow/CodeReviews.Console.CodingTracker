using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker
{
    internal class DatabaseManager
    {
        //Appsetting.json config connection
        public string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            return connectionString;

        }
        
        internal void CreateTable()
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = @"CREATE TABLE IF NOT EXISTS coding_tracker(
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            StartTime TEXT,
                            EndTime TEXT,
                            Duration TEXT)";

                connection.Execute(sql); //Dapper execute
            }
        }

        // Return rows affected checks if the execute succeeds or not
        internal int Post(CodingSession session)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = "INSERT INTO coding_tracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";

                var rowsAffected = connection.Execute(sql,session); //Dapper Execute
                return rowsAffected;
            }
        }

        internal List<CodingSession> Get()
        { 
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = "SELECT * FROM coding_tracker";

                var sessions = connection.Query<CodingSession>(sql);

                return sessions.ToList();
            }
        }


        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = "DELETE FROM coding_tracker WHERE Id = @Id";

                connection.Execute(sql, new {Id = id });
            }
        }

        internal void Update(CodingSession session)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = @"UPDATE coding_tracker 
                            SET StartTime = @StartTime, 
                            EndTime = @EndTime, 
                            Duration = @Duration
                            WHERE Id = @Id";

                connection.Execute(sql, session);
            }
        }

        internal bool CheckIdExists(int id)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = "SELECT COUNT(*) FROM coding_tracker WHERE Id = @Id";

                int count = connection.ExecuteScalar<int>(sql, new { Id = id});

                return count > 0;
            }
        }

        internal CodingSession GetSessionByID(int id)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                var sql = "SELECT * FROM coding_tracker where Id = @Id";
                return connection.QuerySingle<CodingSession>(sql, new { Id = id });
            }

        }
    }
}
