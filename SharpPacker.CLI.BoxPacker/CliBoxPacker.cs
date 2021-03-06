﻿using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using Newtonsoft.Json;
using SharpPacker.Base.Models;
using System;
using System.IO;
using SharpPacker.Strategy.BoxPackerClone;

namespace SharpPacker.CLI.BoxPacker
{
    enum ExitCode : int
    {
        DONE = 0,
        NoArgs = -1,
        ArgumentParseError = -10,
        UnknownPackingError = -80,
    }

    class ParsingTarget
    {
        [ValueArgument(typeof(string), 'i', "input", Description = "Request file path, *.json", Optional = false)]
        public string requestPath = string.Empty;

        [ValueArgument(typeof(string), 'o', "output", Description = "Result file path, *.json", Optional = true)]
        public string resultPath = string.Empty;
    }

    class CliBoxPacker
    {
        static int Main(string[] args)
        {
            var parser = new CommandLineParser.CommandLineParser();
            parser.AcceptSlash = true;

            var cliArgs = new ParsingTarget();
            parser.ExtractArgumentAttributes(cliArgs);

            if (args.Length == 0)
            {
                parser.ShowUsageHeader = "Here is how you use the app: ";
                parser.ShowUsageFooter = "Have fun!";
                parser.ShowUsage();
                Console.WriteLine("=== PRESS ANY KEY FOR QUIT ===");
                Console.ReadKey();
                return (int)ExitCode.NoArgs;
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
                return (int)ExitCode.ArgumentParseError;
            }

            parser.ShowParsedArguments();

            if (string.IsNullOrWhiteSpace(cliArgs.resultPath))
            {
                FileInfo requestFileInfo = new FileInfo(cliArgs.requestPath);
                cliArgs.resultPath = requestFileInfo.FullName;
                cliArgs.resultPath = cliArgs.resultPath.Substring(0, cliArgs.resultPath.LastIndexOf(requestFileInfo.Extension));
                cliArgs.resultPath += $".result{requestFileInfo.Extension}";

                Console.WriteLine($"Result file path: {cliArgs.resultPath}");
            }

            int result = (int)ExitCode.UnknownPackingError;

            Console.WriteLine($"=== START {DateTime.Now}");

            try
            {
                result = (int)ProceedBoxPacker(cliArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine("=== ERROR:");
                Console.WriteLine(e.Message);
                Console.WriteLine("=== PRESS ANY KEY FOR QUIT ===");
                Console.ReadKey();
                return (int)ExitCode.UnknownPackingError;
            }

            Console.WriteLine($"=== DONE  {DateTime.Now}");
            return result;
        }

        static ExitCode ProceedBoxPacker(ParsingTarget cliArgs)
        {
            FileInfo requestFileInfo = new FileInfo(cliArgs.requestPath);
            if (!requestFileInfo.Exists)
            {
                throw new FileNotFoundException($"File not found: {requestFileInfo.FullName}", requestFileInfo.FullName);
            }

            BoxPackerRequest request;

            using (StreamReader file = File.OpenText(requestFileInfo.FullName))
            {
                JsonSerializer serializer = new JsonSerializer();
                request = (BoxPackerRequest)serializer.Deserialize(file, typeof(BoxPackerRequest));
            }

            BoxPackerResult result;

            using (var strategy = new BoxPackerCloneStrategy())
            {
                strategy.options.MaxBoxesToBalanceWeight = 15;
                using(var packer = new SharpPacker.Base.BoxPacker(strategy))
                {
                    result = packer.Pack(request);
                }
            }

            using (StreamWriter file = File.CreateText(cliArgs.resultPath))
            {
                JsonSerializer serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                serializer.Serialize(file, result);
            }

            return ExitCode.DONE;
        }
    }
}
