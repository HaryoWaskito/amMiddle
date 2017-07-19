using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace amMiddle.Models
{
    public class amContext : DbContext
    {
        private const string CONST_DATABASENAME = "amContext.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(String.Format("Filename=./{0}", CONST_DATABASENAME));
        }

        public DbSet<amModel> amModel { get; set; }

        public void CreateData(amModel item)
        {
            string insertQuery = string.Empty;

            insertQuery = string.Format("INSERT INTO amModel ( amModelId, SessionID, ActivityName, ActivityType, InputKey, KeyStrokeCount, MouseClickCount, StartTime, EndTime, IsSuccessSendToServer ) " +
                                        "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                        item.amModelId, item.SessionID, item.ActivityName, item.ActivityType, item.InputKey, item.KeyStrokeCount, item.MouseClickCount, item.StartTime, item.EndTime, item.IsSuccessSendToServer);

            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = CONST_DATABASENAME }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.Transaction = transaction;
                    insertCommand.CommandText = insertQuery;
                    insertCommand.CommandType = System.Data.CommandType.Text;
                    insertCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public amModel RetrieveData(string amModelId)
        {
            amModel result = new amModel();
            string retrieveQuery = String.Format("SELECT * FROM amModel WHERE amModelId = '{0}'", amModelId);
            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = CONST_DATABASENAME }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    DateTime dateTimeResult = new DateTime(1900, 1, 1);
                    amModel recordData = new amModel();
                    var sqlCommand = connection.CreateCommand();
                    sqlCommand.Transaction = transaction;
                    sqlCommand.CommandText = retrieveQuery;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            recordData.amModelId = reader["amModelId"] != null ? reader["amModelId"].ToString() : string.Empty;
                            recordData.SessionID = reader["SessionID"] != null ? int.Parse(reader["SessionID"].ToString()) : 0;
                            recordData.ActivityName = reader["ActivityName"] != null ? reader["ActivityName"].ToString() : string.Empty;
                            recordData.ActivityType = reader["ActivityType"] != null ? reader["ActivityType"].ToString() : string.Empty;
                            recordData.KeyStrokeCount = reader["KeyStrokeCount"] != null ? long.Parse(reader["KeyStrokeCount"].ToString()) : 0;
                            recordData.MouseClickCount = reader["MouseClickCount"] != null ? long.Parse(reader["MouseClickCount"].ToString()) : 0;
                            if (reader["StartTime"] != null && DateTime.TryParseExact(reader["StartTime"].ToString(), "yyyymmdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeResult))
                                recordData.StartTime = dateTimeResult;
                            if (reader["EndTime"] != null && DateTime.TryParseExact(reader["EndTime"].ToString(), "yyyymmdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeResult))
                                recordData.EndTime = dateTimeResult;
                        }
                    }
                    transaction.Commit();
                }
            }
            return result;
        }

        public void UpdateData(amModel item)
        {            
            string updateQuery = string.Empty;

            updateQuery = string.Format("UPDATE amModel SET KeyStrokeCount = '{0}', MouseClickCount = '{1}', StartTime = '{2}', EndTime = '{3}' " +
                                        "WHERE amModelId = '{4}'", item.KeyStrokeCount, item.MouseClickCount, item.StartTime, item.EndTime, item.amModelId);

            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = CONST_DATABASENAME }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var updateCommand = connection.CreateCommand();
                    updateCommand.Transaction = transaction;
                    updateCommand.CommandText = updateQuery;
                    updateCommand.CommandType = System.Data.CommandType.Text;
                    updateCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public void DeleteDataSuccessSendToServer()
        {
            string deleteQuery = "DELETE amModel WHERE item.IsSuccessSendToServer = 'True'";

            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = CONST_DATABASENAME }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var deleteCommand = connection.CreateCommand();
                    deleteCommand.Transaction = transaction;
                    deleteCommand.CommandText = deleteQuery;
                    deleteCommand.CommandType = System.Data.CommandType.Text;
                    deleteCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        public amModel GetLastActiveData()
        {
            amModel result = new amModel();
            string retrieveQuery = String.Format("SELECT * FROM amModel ORDER BY StartTime DESC LIMIT 1");
            using (var connection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = CONST_DATABASENAME }))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    DateTime dateTimeResult = new DateTime(1900, 1, 1);
                    amModel recordData = new amModel();
                    var sqlCommand = connection.CreateCommand();
                    sqlCommand.Transaction = transaction;
                    sqlCommand.CommandText = retrieveQuery;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            recordData.amModelId = reader["amModelId"] != null ? reader["amModelId"].ToString() : string.Empty;
                            recordData.SessionID = reader["SessionID"] != null ? int.Parse(reader["SessionID"].ToString()) : 0;
                            recordData.ActivityName = reader["ActivityName"] != null ? reader["ActivityName"].ToString() : string.Empty;
                            recordData.ActivityType = reader["ActivityType"] != null ? reader["ActivityType"].ToString() : string.Empty;
                            recordData.InputKey = reader["InputKey"] != null ? reader["InputKey"].ToString() : string.Empty;
                            recordData.KeyStrokeCount = reader["KeyStrokeCount"] != null ? long.Parse(reader["KeyStrokeCount"].ToString()) : 0;
                            recordData.MouseClickCount = reader["MouseClickCount"] != null ? long.Parse(reader["MouseClickCount"].ToString()) : 0;
                            if (reader["StartTime"] != null && DateTime.TryParseExact(reader["StartTime"].ToString(), "yyyymmdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeResult))
                                recordData.StartTime = dateTimeResult;
                            if (reader["EndTime"] != null && DateTime.TryParseExact(reader["EndTime"].ToString(), "yyyymmdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTimeResult))
                                recordData.EndTime = dateTimeResult;
                        }
                    }
                    transaction.Commit();
                }
            }
            return result;
        }
    }
}
