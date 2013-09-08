using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NV.LogUtils.Common;
using NV.LogUtils.Common.Analyzers;
using NV.LogUtils.Common.Logs;
using NV.LogUtils.Common.Reporters;
using NV.LogUtils.MSBuild.Processors;
using NV.LogUtils.MSBuild.Reporters;

namespace NV.LogUtils.MSBuild.WarningsCounter
{
    class Program
    {
        private static IProcessor _processor;
        private static IAnalyzer _analyzer;
        private static IReporter _reporter;
        private static string _logFile;
        private static bool _waitKeyPressed;
        private static bool _install;

        public const decimal MaxExpceptionDepth = 3;

        static void Main ( string[] args )
        {
            try
            {
                Setup(args);

                if (_install)
                {
                    Install();
                }
                else
                {
                    Validate();

                    IList<IEntry> entries;

                    using (var log = new LogStreamReader(_logFile))
                    {
                        entries = _processor.GetEntries(log);
                    }

                    var report = _analyzer.Analyze(entries);
                    Console.WriteLine(_reporter.PrintReport(report));
                }
            } catch (Exception e)
            {
                PrintException(e);
            }


            if (_waitKeyPressed)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }


        }

        private static void Install()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = assembly.Location;
            Environment.SetEnvironmentVariable("WarnCounterPath", path, EnvironmentVariableTarget.User);

            Console.WriteLine("Installation completed.");
            Console.WriteLine("You should logoff and logon now");

        }

        private static void Validate ( )
        {
            if (!_install && string.IsNullOrEmpty(_logFile))
                throw new ArgumentException("No input file specified!");
        }

        private static void Setup ( string[] args )
        {
            _install = args.Any(x => x == "/install");
            _waitKeyPressed = args.Any(x => (x == "/i") || (x == "/interactive"));

            _processor = new MSBuildLogProcessor();
            _analyzer = new SimpleAnalyzer();

            if (args.Contains("/csv"))
            {
                SetupCSVReporter(args);
            }
            else if(args.Contains("/tc"))
            {
                _reporter = new TeamCityReporter();
            }
            else
            {
                _reporter = new SimpleReporter();
            }

            _logFile = args.FirstOrDefault(File.Exists);
        }

        private static void SetupCSVReporter(IEnumerable<string> args)
        {
            var reporter = new CSVReporter();
            var delimeter = args.FirstOrDefault(x => x.StartsWith("/csv-field-separator:"));
            
            if (!string.IsNullOrEmpty(delimeter))
            {
                if (delimeter.Length <= "/csv-field-separator:".Length)
                    throw new ArgumentException("Empty csv row separator specified!");

                reporter.FieldSeparator = delimeter.Substring("/csv-field-separator:".Length);
            }
            _reporter = reporter;
        }

        private static void PrintException ( Exception exception, int depth = 0 )
        {
            if (depth == 0)
            {
                Console.WriteLine("===========");
                Console.WriteLine("Fatal: Unhandled exception occured!");
            }

            Console.WriteLine("----------");
            Console.WriteLine(exception.Message);
            if (exception.InnerException != null)
            {
                if (depth < MaxExpceptionDepth)
                {
                    PrintException(exception.InnerException, depth + 1);
                }
                else
                {
                    Console.WriteLine("... and so on ...");
                }
            }
            Console.WriteLine("----------");

            if (depth == 0)
            {
                Console.WriteLine();
                Console.WriteLine("===========");
            }
        }
    }
}
