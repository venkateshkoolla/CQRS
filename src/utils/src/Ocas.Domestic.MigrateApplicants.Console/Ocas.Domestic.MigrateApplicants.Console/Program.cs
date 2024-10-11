using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Ocas.Identity.Auth.Clients;
using Ocas.Identity.Core.Configuration;

namespace Ocas.Domestic.MigrateApplicants.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new Exception("Must provide environment in command line arguments");
            }

            System.Console.WriteLine($"Executing migration in {args[0]} environment");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{args[0]}.json", false, true);
            var configuration = builder.Build();

            var crmConnStr = new SqlConnectionStringBuilder(configuration.GetConnectionString("OCAS_MSCRM"));
            var identityConnStr = new SqlConnectionStringBuilder(configuration.GetConnectionString("ocasidentity"));
            var adldsSettings = new AdldsSettings
            {
                Domain = configuration["ocas:adldsSettings:domain"],
                IdentityConnectionString = identityConnStr.ToString()
            };

            var client = new AdldsClient(adldsSettings);

            ///////////////////////////
            // Get Users From AD LDS //
            ///////////////////////////
            System.Console.WriteLine($"Fetching users from {adldsSettings.Domain}...");

            var users = client.FindAll();

            System.Console.WriteLine("Done");

            if (users is null || !users.Any())
            {
                throw new Exception("Failed to retrieve any users");
            }

            //////////////////////////////
            // Create Users in Identity //
            //////////////////////////////
            System.Console.Write($"Creating {users.Count} users in {identityConnStr.DataSource}...");

            client.CreateAspNetIdentityUsers(users);

            System.Console.WriteLine("Done");

            ////////////////////////////
            // Update Contacts in CRM //
            ////////////////////////////
            IList<string> usernames;
            using (var conn = new SqlConnection(crmConnStr.ToString()))
            {
                conn.Open();

                const string sql = "SELECT DISTINCT ocaslr_username FROM dbo.ContactBase WHERE ocaslr_userid IS NULL AND StateCode = 0 AND OCASLR_ContactType = 1 AND ocaslr_username IS NOT NULL";

                usernames = conn.Query<string>(sql, commandType: CommandType.Text) as IList<string>;
            }

            System.Console.WriteLine($"Updating {usernames.Count} Contacts in {identityConnStr.DataSource}...");

            using (var conn = new SqlConnection(crmConnStr.ToString()))
            {
                conn.Open();

                conn.Execute(@"CREATE TABLE #AdldsUserMigration (OCASLR_UserName NVARCHAR(100) collate Latin1_General_CI_AI, ocaslr_userid NVARCHAR(100) collate Latin1_General_CI_AI);");

                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.BulkCopyTimeout = 300;
                    bulkCopy.DestinationTableName = "#AdldsUserMigration";

                    var dt = new DataTable("AdldsUserMigration");
                    dt.Columns.Add("OCASLR_UserName", typeof(string));
                    dt.Columns.Add("ocaslr_userid", typeof(string));

                    foreach (var username in usernames)
                    {
                        if (users.TryGetValue(username, out var user))
                            dt.Rows.Add(username, user.Id.ToString());
                    }

                    bulkCopy.WriteToServer(dt);
                }

                var rowsAffected = conn.Execute(@"
UPDATE 
    ContactBase
SET 
    ContactBase.ocaslr_userid = #AdldsUserMigration.[ocaslr_userid]
FROM 
    ContactBase
    INNER JOIN #AdldsUserMigration ON ContactBase.OCASLR_UserName = #AdldsUserMigration.OCASLR_UserName
WHERE ContactBase.ocaslr_userid IS NULL
    AND ContactBase.StateCode = 0
    AND ContactBase.OCASLR_ContactType = 1
    AND ContactBase.ocaslr_username IS NOT NULL;");

                conn.Execute(@"DROP TABLE #AdldsUserMigration;", commandTimeout: 300);
                System.Console.WriteLine($"{rowsAffected} Rows Affected");
            }

            System.Console.WriteLine("Done");
        }
    }
}
