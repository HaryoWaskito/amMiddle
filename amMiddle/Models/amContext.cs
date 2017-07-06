using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace amMiddle.Models
{
    public class amContext : DbContext
    {
        //public amContext(DbContextOptions<amContext> options) : base(options)
        //{


        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "amContext.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        public DbSet<amModel> amModels { get; set; }

        public void SaveData()
        {
            string insertQuery = @"INSERT INTO amModels ( amModelId, KeyLogCatch, TimeStamp, UserID ) VALUES {0}";
            string insertValue = string.Empty;

            
            {
                insertValue = string.Format("({0},{1},{2},{3}){4}", item.amModelId, item.KeyLogCatch, item.TimeStamp, item.userID,);
            }

            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = "amContext.db" }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandText = insertQuery;
                    insertCommand.Parameters.AddWithValue("$text", "Hello, World!");
                    insertCommand.ExecuteNonQuery();

                    //var selectCommand = connection.CreateCommand();
                    //selectCommand.Transaction = transaction;
                    //selectCommand.CommandText = "SELECT text FROM message";
                    //using (var reader = selectCommand.ExecuteReader())
                    //{
                    //    while (reader.Read())
                    //    {
                    //        var message = reader.GetString(0);

                    //    }
                    //}

                    transaction.Commit();
                }
            }
        }
    }
}
