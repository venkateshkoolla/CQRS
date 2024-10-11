using System;
using System.Diagnostics;
using ManyConsole;

namespace Ocas.Domestic.Crm.Db.Cli.Commands
{
    /// <summary>
    /// Displays various versions
    /// </summary>
    ///
    /// <example>
    /// as version
    /// </example>
    public class VersionCommand : ConsoleCommand
    {
        public VersionCommand()
        {
            IsCommand("version", "Display version details.");
        }

        public override int Run(string[] remainingArguments)
        {
            var assembly = typeof(Program).Assembly;
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.WriteLine($"As Version: {fvi.FileVersion}");
            Console.WriteLine($"CLR Version: {Environment.Version}");

            return 0;
        }
    }
}
