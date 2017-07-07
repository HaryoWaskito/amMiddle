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
            string insertValue = string.Empty;

            foreach (var item in amModels)
                insertValue = string.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                    item.amModelId, item.appExePath, item.InputType, item.InputClickedCounter, item.TimeStamp, item.userID);

            insertValue.Remove(insertValue.Length - 1);

            string insertQuery = string.Format("INSERT INTO amModels ( amModelId, appExePath, InputType, InputClickedCounter, TimeStamp, UserID ) VALUES {0}", insertValue);

            string localQuery = string.Empty;
            foreach (var item in amModels)
            {
                localQuery = @" IF 1 = ( SELECT TOP 1 1 FROM amModel WHERE InputType = 'Keyboard' AND appExePath = '" + item.appExePath + "') +" +
                               " BEGIN " +
                               " UPDATE amModel SET KeyLogCatch =  " +
                               " END " +
                               ";";
            }


            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = "amContext.db" }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandText = insertQuery;
                    insertCommand.CommandType = System.Data.CommandType.Text;
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
