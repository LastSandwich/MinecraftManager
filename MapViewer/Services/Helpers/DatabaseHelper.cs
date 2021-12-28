namespace MapViewer.Services.Helpers
{
    using System.Reflection;

    using DbUp;
    using DbUp.Engine;

    using MapViewer.Services.SeedDatabase;

    using NPoco.SqlServer;

    public static class DatabaseHelper
    {
        public static void ResetDatabase(string connectionString)
        {
            const string Sql = @"DECLARE @@Sql NVARCHAR(500) DECLARE @@Cursor CURSOR
            SET @@Cursor = CURSOR FAST_FORWARD FOR
            SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_SCHEMA + '].[' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + '];'
            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
            LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME = rc1.CONSTRAINT_NAME

            OPEN @@Cursor FETCH NEXT FROM @@Cursor INTO @@Sql

            WHILE(@@@@FETCH_STATUS = 0)
            BEGIN
                Exec sp_executesql @@Sql
            FETCH NEXT FROM @@Cursor INTO @@Sql
            END

            CLOSE @@Cursor DEALLOCATE @@Cursor;

            EXEC sp_MSforeachtable 'DROP TABLE ?';";

            using var db = new SqlServerDatabase(connectionString);
            db.Execute(Sql);

            Thread.Sleep(2000);
        }

        public static DatabaseUpgradeResult UpgradeDatabase(string connectionString, Assembly assembly)
        {
            var upgradeEngine = DeployChanges.To.SqlDatabase(connectionString)
                .WithExecutionTimeout(new TimeSpan(0, 2, 0))
                .WithScriptsEmbeddedInAssembly(assembly)
                .LogToConsole()
                .Build();
            return upgradeEngine.PerformUpgrade();
        }

        public static async Task SeedDatabase(IEnumerable<ISeedData> seedCreators)
        {
            foreach (var seedCreator in seedCreators)
            {
                await seedCreator.Process();
            }
        }
    }
}
