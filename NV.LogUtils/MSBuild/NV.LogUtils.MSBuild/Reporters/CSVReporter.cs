using System.Globalization;
using System.Linq;
using NV.LogUtils.Common;
using NV.LogUtils.MSBuild.Entries;

namespace NV.LogUtils.MSBuild.Reporters
{
    public class CSVReporter : IReporter
    {
        public string PrintReport ( IReport report )
        {
            var sorted = report.Entries.ToList();
            sorted.Sort(( x, y ) => -1 * (x.Value - y.Value));

            string output = "File, Line, Warning, Message, Count\r\n";
            foreach (var pair in sorted)
            {
                var warning = pair.Key as MSBuildWarning;
                if (warning != null)
                {
                    output += Escape(warning.File) + ',';
                    output += Escape(warning.Line.ToString(CultureInfo.InvariantCulture)) + ',';
                    output += Escape(warning.Warning) + ',';
                    output += Escape(warning.Message) + ',';
                    output += pair.Value + "\r\n";
                }
            }

            return output;
        }

        private static string Escape ( string text )
        {

            return (text.Contains("\""))
                       ? "\"" + text.Replace("\"", "\"\"") + "\""
                       : text;

        }
    }
}