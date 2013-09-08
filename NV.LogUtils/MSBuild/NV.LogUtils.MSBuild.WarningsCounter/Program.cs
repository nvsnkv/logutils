using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NV.LogUtils.Common;
using NV.LogUtils.Common.Analyzers;
using NV.LogUtils.Common.Logs;
using NV.LogUtils.Common.Reporters;
using NV.LogUtils.MSBuild.Processors;

namespace NV.LogUtils.MSBuild.WarningsCounter
{
    class Program
    {
        private static MSBuildLogProcessor _processor;
        private static SimpleAnalyzer _analyzer;
        private static SimpleReporter _reporter;
        private static string _logFile;

        public const decimal MaxExpceptionDepth = 3;

        static void Main ( string[] args )
        {
            try
            {
                Setup(args);
                Validate();

                IList<IEntry> rawEntries;

                using (var log = new LogStreamReader(_logFile))
                {
                    rawEntries = _processor.GetEntries(log);
                }

                var report = _analyzer.Analyze(rawEntries);
                Console.WriteLine(_reporter.PrintReport(report));
            }
            catch (Exception e)
            {
                PrintException(e);
            }

#if DEBUG
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
#endif
        }

        private static void Validate()
        {
            if (string.IsNullOrEmpty(_logFile))
                throw new ArgumentException("No input file specified!");
        }

        private static void Setup(string[] args)
        {
            _processor = new MSBuildLogProcessor();
            _analyzer = new SimpleAnalyzer();
            _reporter = new SimpleReporter();

            _logFile = args.FirstOrDefault(File.Exists);
        }

        private static void PrintException(Exception exception, int depth = 0)
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
