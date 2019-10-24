using System;
using System.Data.Entity;
using System.Transactions;
using MainProject.Data;

namespace MainProject.Framework.DatabaseFramework
{
    public class ReGenerateDb
    {
        public static string Init()
        {
            Database.SetInitializer<MainDbContext>(null);
            var dbContext = new MainDbContext();
            var message = ClearDb(dbContext);

            var dbInitialize = new InitEntitiesDb();
            if (string.IsNullOrEmpty(message))
            {
                dbInitialize.InitializeDatabase(dbContext);
            }
            return message;
        }

        public static string ClearDb(MainDbContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                var message = string.Empty;
                try
                {
                    // remove all tables
                    context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"DROP TABLE ?\"");
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                if (!string.IsNullOrEmpty(message)) return message;
            }
            else
            {
                return "No database instance";
            }
            return string.Empty;
        }
    }
}
