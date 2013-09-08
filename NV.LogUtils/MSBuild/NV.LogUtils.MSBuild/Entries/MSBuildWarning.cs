using System;
using System.Text.RegularExpressions;
using NV.LogUtils.Common;

namespace NV.LogUtils.MSBuild.Entries
{
    public class MSBuildWarning : IEntry
    {
        private static readonly Regex WarningRegex = new Regex(@"^(?<File>.*?)\((?<Line>\d*?)\)\s*?:\s*?warning\s*?(?<Warning>.*?):(?<Message>.*?)$");

        public string File
        {
            get { return _match.Groups["File"].Value; }
        }

        public int Line
        {
            get { return int.Parse(_match.Groups["Line"].Value); }
        }

        public string Warning
        {
            get { return _match.Groups["Warning"].Value; }
        }

        public string Message
        {
            get { return _match.Groups["Message"].Value; }
        }

        public MSBuildWarning ( string logLine )
        {
            _match = WarningRegex.Match(logLine);
            if (!_match.Success)
                throw new ArgumentException("Given string is not a warning!");

            _warningLine = logLine;
        }

        public static MSBuildWarning CreateWarning ( string text )
        {
            return (CanCreateWarning(text))
                       ? new MSBuildWarning(text)
                       : null;
        }

        public static bool CanCreateWarning(string text)
        {
            return WarningRegex.IsMatch(text);
        }

        public override string ToString ( )
        {
            return _warningLine;
        }

        #region Equality members

        protected bool Equals ( MSBuildWarning other )
        {
            return string.Equals(_warningLine, other._warningLine);
        }

        public override int GetHashCode ( )
        {
            return (_warningLine != null ? _warningLine.GetHashCode() : 0);
        }

        public override bool Equals ( object obj )
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MSBuildWarning)obj);
        }

        #endregion

        private readonly string _warningLine;

        private readonly Match _match;
    }
}
