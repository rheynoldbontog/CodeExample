using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using MySql.Data.MySqlClient;

namespace SSG.Data.Initializers
{
    public class CreateTablesIfNotExist<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        private readonly string[] _tablesToValidate;
        private readonly string[] _customCommands;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tablesToValidate">A list of existing table names to validate; null to don't validate table names</param>
        /// <param name="customCommands">A list of custom commands to execute</param>
        public CreateTablesIfNotExist(string[] tablesToValidate, string[] customCommands)
            : base()
        {
            this._tablesToValidate = tablesToValidate;
            this._customCommands = customCommands;
        }

        public void InitializeDatabase(TContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                bool createTables = false;
                if (_tablesToValidate != null && _tablesToValidate.Length > 0)
                {
                    //we have some table names to validate
                    var existingTableNames = new List<string>(context.Database.SqlQuery<string>(string.Format("SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' AND TABLE_SCHEMA = '{0}'", context.Database.Connection.Database)));
                    createTables = existingTableNames.Intersect(_tablesToValidate, StringComparer.InvariantCultureIgnoreCase).Count() == 0;
                }
                else
                {
                    //check whether tables are already created
                    int numberOfTables = 0;
                    foreach (var t1 in context.Database.SqlQuery<int>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' "))
                        numberOfTables = t1;

                    createTables = numberOfTables == 0;
                }

                if (createTables)
                {
                    //create all tables
                    var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();

                    //Need to fix some of the script for MySql
                    //Need to compare by name for some reason starting with MySql.Data 6.6.4
                    if (context.Database.Connection.GetType().Name == typeof(MySqlConnection).Name)
                    {
                        //MySql doesn't support varbinary(MAX) so it generates the script with varbinary only without
                        //a size specified, so change to longblob...could probably do this in the mapping for these properties instead
                        //dbCreationScript = dbCreationScript.Replace("`BinaryFieldName` varbinary,", "`PictureBinary` LONGBLOB,");
                        dbCreationScript = dbCreationScript.Replace("`DownloadBinary` varbinary,", "`DownloadBinary` LONGBLOB,");

                        dbCreationScript = dbCreationScript.Replace("`PictureBinary` varbinary,", "`PictureBinary` LONGBLOB,");

                        // Some setups doesn't support NVARCHAR (65535), replace with MEDIUMTEXT
                        dbCreationScript = dbCreationScript.Replace("nvarchar (65535)", "MEDIUMTEXT");

                        // Surround ORDER
                        dbCreationScript = dbCreationScript.Replace(" Order ", " `Order` ");

                        //Some of the constraint names are too long for MySql, so shorten them
                        dbCreationScript = dbCreationScript.Replace("PollVotingRecord_TypeConstraint_From_UserContent_To_PollVotingRecord", "PVR_TypeConstraint_UC");
                        dbCreationScript = dbCreationScript.Replace("NewsComment_TypeConstraint_From_UserContent_To_NewsComment", "NC_TypeConstraint_UC");

                    }

                    context.Database.ExecuteSqlCommand(dbCreationScript);

                    //Seed(context);
                    context.SaveChanges();

                    if (_customCommands != null && _customCommands.Length > 0)
                    {
                        foreach (var command in _customCommands)
                            context.Database.ExecuteSqlCommand(command);
                    }
                }
            }
            else
            {
                throw new ApplicationException("No database instance");
            }
        }
    }
}
