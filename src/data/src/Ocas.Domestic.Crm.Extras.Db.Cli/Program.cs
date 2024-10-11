using System;
using System.Collections.Generic;
using ManyConsole;

namespace Ocas.Domestic.Crm.Db.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var commands = GetCommands();

            var result = 1;
            try
            {
                result = ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
            }
            catch (Exception e)
            {
                // Swallow bad errors
                Console.WriteLine(e);
            }

#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
#endif

            return result;
        }

        private static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
