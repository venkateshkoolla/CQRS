using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ManyConsole;
using Ocas.Domestic.Crm.Db.Cli.Services;

namespace Ocas.Domestic.Crm.Db.Cli.Commands
{
    /// <summary>
    /// Purges database of all schema.
    /// </summary>
    ///
    /// <example>
    /// as purge -v
    /// </example>
    public class PurgeCommand : ConsoleCommand
    {
        private string ConnectionString { get; set; }
        private bool Force { get; set; }
        private bool Verbose { get; set; }

        public PurgeCommand()
        {
            IsCommand("purge", "Purges database.");

            HasLongDescription("Purges the target database removing all schema objects " +
                               "including tables, stored procs and sequences.");

            HasOption("c|connectionString:", "Connection string to use.", x => ConnectionString = x);
            HasOption("f|force", "Do not request user confirmation", x => Force = true);
            HasOption("v|verbose", "Include verbose output.", x => Verbose = true);
        }

        public override int Run(string[] remainingArguments)
        {
            return RunAsync().GetAwaiter().GetResult();
        }

        private async Task<int> RunAsync()
        {
            if (!Force)
            {
                Console.WriteLine("ARE YOU SURE YOU WANT TO PURGE THE DATABASE?? [Y/N]");
                var confirmRes = Console.ReadKey(true);
                var confirmKeys = new[] { 'Y', 'y' };

                if (!confirmKeys.Contains(confirmRes.KeyChar))
                {
                    Console.WriteLine("Purge cancelled.. phew!");
                    return 0;
                }
            }

            Console.WriteLine("Purging database");

            // Use connection string passed as arg or fallback to config if not provided
            var connectionString = ConnectionString ?? ConfigurationManager.ConnectionStrings["translations"].ConnectionString;

            var purger = new DbPurger(connectionString);

            if (Verbose)
            {
                purger.Log = Console.WriteLine;
            }

            await purger.Purge();

            Console.WriteLine("Purge complete!");

            return 0;
        }
    }
}