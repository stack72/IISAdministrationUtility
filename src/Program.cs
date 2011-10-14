using System;
using System.Collections.Generic;
using Autofac;

namespace IISAdministration
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentNullException("args", "You need to pass a site name and an appPool at the very least");

            if (args[0] == "help")
            {
                //print something back to the screen here
                Console.ReadLine();
            }
            else
            {
                var commandLineArguements = ParseCommandLineArguements(args);

                using (var container = Bootstrap.Components())
                {
                    var iisManager = container.Resolve<IIisManager>();
                    iisManager.CreateAppPool(commandLineArguements);
                    iisManager.CreateSite(commandLineArguements);
                }
            }
        }

        private static Dictionary<string, string> ParseCommandLineArguements(string[] args)
        {
            var commandLineArguements = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                var key = arg.Substring(1, arg.IndexOf(":")-1);
                var value = arg.Substring(arg.IndexOf(":")+1, arg.Length - (arg.IndexOf(":")+1));

                commandLineArguements.Add(key, value);
            }

            return commandLineArguements;

        }
    }
}
