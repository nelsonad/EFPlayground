using EFPlayground.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Tests
{
    public static class TestUtilities
    {
        public static void ResetDatabase()
        {
            string connectionString = "server=.;initial catalog=EFPlayground;integrated security=true;";
            DropDatabase(connectionString);

            var initializer = new MigrateDatabaseToLatestVersion<EFPlaygroundContext, EFPlayground.Data.Migrations.Configuration>();

            Database.SetInitializer(initializer);

            using (var context = new EFPlaygroundContext())
            {
                context.Database.Initialize(true);
            }
        }

        private static void DropDatabase(string connectionString)
        {
            const string DropDatabaseSql =
        "if (select DB_ID('{0}')) is not null\r\n"
        + "begin\r\n"
        + "alter database [{0}] set offline with rollback immediate;\r\n"
        + "alter database [{0}] set online;\r\n"
        + "drop database [{0}];\r\n"
        + "end";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var sqlToExecute = String.Format(DropDatabaseSql, connection.Database);

                    var command = new SqlCommand(sqlToExecute, connection);

                    Console.WriteLine("Dropping database");
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database is dropped");
                }
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Message.StartsWith("Cannot open database"))
                {
                    Console.WriteLine("Database does not exist.");
                    return;
                }
                throw;
            }
        }
    }
}
