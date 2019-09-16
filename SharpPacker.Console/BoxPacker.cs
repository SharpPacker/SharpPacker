using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using System;
using System.IO;

namespace SharpPacker.CLI
{
    class ParsingTarget
    {
        [EnumeratedValueArgument(typeof(string), 'm', "mode", AllowedValues = "box;pallet", Description = "Mode = \"BOX\" or \"PALLET\"", AllowMultiple = false, IgnoreCase = true, Optional = false)]
        public string mode;

        [ValueArgument(typeof(string), 'i', "input", Description = "Request file path, *.json", Optional = false)]
        public string requestPath;

        [ValueArgument(typeof(string), 'o', "output", Description = "Result file path, *.json", Optional = true)]
        public string resultPath;
    }

    class BoxPacker
    {
        static void Main(string[] args)
        {

            var parser = new CommandLineParser.CommandLineParser();
            parser.AcceptSlash = true;

            var cliArgs = new ParsingTarget();
            parser.ExtractArgumentAttributes(cliArgs);

            if(args.Length == 0)
            {
                parser.ShowUsageHeader = "Here is how you use the app: ";
                parser.ShowUsageFooter = "Have fun!";
                parser.ShowUsage();
                return;
            }

            try
            {
                parser.ParseCommandLine(args);
            }
            catch (UnknownArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (CommandLineException e)
            {
                Console.WriteLine("=== ERROR:");
                Console.WriteLine(e.Message);
                Console.WriteLine("=== PRESS ANY KEY FOR QUIT ===");
                Console.ReadKey();
                return;
            }

            parser.ShowParsedArguments();

            if (string.IsNullOrWhiteSpace(cliArgs.resultPath))
            {
                FileInfo requestFileInfo = new FileInfo(cliArgs.requestPath);
                cliArgs.resultPath = requestFileInfo.FullName;
                cliArgs.resultPath = cliArgs.resultPath.Substring(0, cliArgs.resultPath.LastIndexOf(requestFileInfo.Extension));
                cliArgs.resultPath += $"_result{requestFileInfo.Extension}";

                Console.WriteLine($"Result file path: {cliArgs.resultPath}");
            }

            try
            {
                Proceed(cliArgs);
            }
            catch(Exception e)
            {
                Console.WriteLine("=== ERROR:");
                Console.WriteLine(e.Message);
                Console.WriteLine("=== PRESS ANY KEY FOR QUIT ===");
                Console.ReadKey();
                return;
            }
            
        }

        static void Proceed(ParsingTarget cliArgs)
        {
            FileInfo requestFileInfo = new FileInfo(cliArgs.requestPath);
            if (!requestFileInfo.Exists)
            {
                throw new FileNotFoundException("File not found: ", cliArgs.requestPath);
            }

            Console.WriteLine(requestFileInfo.FullName);
            Console.WriteLine(cliArgs.resultPath);
        }
    }
}
