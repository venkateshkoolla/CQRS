using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using ManyConsole;
using Microsoft.SqlServer.Dac;

namespace Ocas.Domestic.Crm.Db.Cli.Commands
{
    /// <summary>
    /// Deploys dacpac file either creating a new database or updating schema of existing.
    /// </summary>
    ///
    /// <example>
    /// as deploy --file="..\..\..\..\Ocas.Domestic.Crm.Extras.Db.Ssdt\bin\Debug\Ocas.Domestic.Crm.Extras.Db.Ssdt.dacpac" -v -e "dev" -d "OCAS_MSCRM" -i "ocasintsql"
    /// </example>
    public class DeployCommand : ConsoleCommand
    {
        private string Environment { get; set; }
        private string DacpacFilePath { get; set; }
        private string CrmDatabase { get; set; }
        private string CrmServer { get; set; }
        private string ConnectionString { get; set; }
        private bool Backup { get; set; }
        private bool Script { get; set; }
        private bool Verbose { get; set; }

        public DeployCommand()
        {
            IsCommand("deploy", "Deploys a dacpac file.");

            HasLongDescription("This will deploy a dacpac file to the target database " +
                               "and optionally generate a change script instead of running it.");

            HasRequiredOption("f|file=", "The dacpac file to deploy.", x => DacpacFilePath = x);
            HasRequiredOption("e|env=", "The environment to deploy to.", x => Environment = x);
            HasRequiredOption("i|instance=", "The CRM server to query to.", x => CrmServer = x);

            HasOption("c|connectionString=", "Connection string to use.", x => ConnectionString = x);
            HasOption("d|crm=", "The CRM database to query to.", x => CrmDatabase = x);
            HasOption("b|backup", "Backup prior to changes being applied.", _ => Backup = true);
            HasOption("s|script", "Generate change script but not run.", _ => Script = true);
            HasOption("v|verbose", "Include verbose output.", _ => Verbose = true);
        }

        public override int Run(string[] remainingArguments)
        {
            return RunAsync().GetAwaiter().GetResult();
        }

        private async Task<int> RunAsync()
        {
            // Ensure dacpac file exists
            var dacFileInfo = new FileInfo(DacpacFilePath);
            if (!dacFileInfo.Exists)
            {
                throw new Exception("Invalid file path");
            }

            Console.WriteLine("Starting deploy of dacpac");

            // Use connection string passed as arg or fallback to config if not provided
            var connectionString = ConnectionString ?? ConfigurationManager.ConnectionStrings["Crm.Extras"].ConnectionString;

            // Extract target database from connection string
            var builder = new SqlConnectionStringBuilder(connectionString);
            var targetDatabaseName = builder.InitialCatalog;

            Console.WriteLine($"Target database: `{targetDatabaseName}`");

            // Start deploy
            var dacServices = new DacServices(connectionString);
            var package = DacPackage.Load(dacFileInfo.FullName);

            var options = new DacDeployOptions
            {
                //DropObjectsNotInSource = true,
                BlockWhenDriftDetected = true,
                BlockOnPossibleDataLoss = false,
                BackupDatabaseBeforeChanges = Backup,
                ScriptDatabaseOptions = false
            };
            options.SqlCommandVariableValues.Add("Environment", Environment);
            options.SqlCommandVariableValues.Add("SERVER", CrmServer);

            if (Environment == "dev")
            {
                options.SqlCommandVariableValues.Add("OCAS_MSCRM", CrmDatabase);
            }

            if (Verbose)
            {
                dacServices.Message += (sender, eventArgs) => Console.WriteLine(eventArgs.Message.Message);
            }

            if (Script)
            {
                var now = DateTime.Now;
                var filename = $"{now.Year}{now.Month}{now.Day}{now.Hour}{now.Minute}{now.Second}_ChangeScript.sql";
                var sql = dacServices.GenerateDeployScript(package, targetDatabaseName, options);
                using (var sw = new StreamWriter(filename))
                {
                    await sw.WriteAsync(sql);
                }

                var scriptFileInfo = new FileInfo(filename);
                Console.WriteLine($"Dacpac change script written to `{scriptFileInfo.FullName}`");
            }
            else
            {
                dacServices.Deploy(package, targetDatabaseName, true, options);
                Console.WriteLine("Dacpac deploy complete!");
            }

            return 0;
        }
    }
}