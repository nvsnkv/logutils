using System.Globalization;
using System.Linq;
using NV.LogUtils.Common;
using NV.LogUtils.MSBuild.Entries;

namespace NV.LogUtils.MSBuild.Reporters
{
    public class CSVReporter : IReporter
    {
        public CSVReporter ( )
        {
            FieldSeparator = ",";
        }

        public string PrintReport ( IReport report )
        {
            var sorted = report.Entries.ToList();
            sorted.Sort(( x, y ) => -1 * (x.Value - y.Value));

            var output = "File" + FieldSeparator + "Line" + FieldSeparator + "Warning" + FieldSeparator + "Message"+ FieldSeparator + "Count\r\n";
            foreach (var pair in sorted)
            {
                var warning = pair.Key as MSBuildWarning;
                if (warning != null)
                {
                    output += Escape(warning.File) + FieldSeparator;
                    output += Escape(warning.Line.ToString(CultureInfo.InvariantCulture)) + FieldSeparator;
                    output += Escape(warning.Warning) + FieldSeparator;
                    output += Escape(warning.Message) + FieldSeparator;
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

        public string FieldSeparator { get; set; }
    }
}