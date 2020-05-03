using Kantin.Data;
using Microsoft.EntityFrameworkCore;

namespace Kantin.Tests.Utils
{
    public class DbContextCreator
    {
        /*
         * True: if you would like to test the change in fake/in memory database
         * False: if you would like to test the change in real docker database
         * Note: DO NOT COMMIT any change, if you changed this.
         */
        public readonly bool UseInMemoryDatabase = false;

        /*
         * Connection string to current docker SQL server
         * Note: 
         * - Change this if you would like to test this in you own database
         * - Make sure the database is fully updated 
         * - DO NOT COMMIT any change, if you changed this.
         */
        public readonly string SqlServerConnectionString = "Server=localhost,1433;Initial Catalog=KantinDB;User ID=sa;Password=123Kantin!@#";

        public KantinEntities CreateDbContext()
        {
            if (UseInMemoryDatabase)
                return CreateInMemoryDbContext();
            else
                return CreateConnectionToDbContext();
        }

        private KantinEntities CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<KantinEntities>()
                            .UseInMemoryDatabase(databaseName: "InMemoryArticleDatabase")
                            .Options;

            return new KantinEntities(options);
        }

        private KantinEntities CreateConnectionToDbContext()
        {
            var options = new DbContextOptionsBuilder<KantinEntities>()
                            .UseSqlServer(SqlServerConnectionString)
                            .Options;

            return new KantinEntities(options);
        }
    }
}
