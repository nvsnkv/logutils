using System;
using System.IO;

namespace NV.LogUtils.Common.Logs
{
    public class LogStreamReader : StreamReader, ILog
    {
        public LogStreamReader ( Stream stream )
            : base(stream)
        {
            LinesReaded = 0;
        }

        public LogStreamReader ( string path )
            : this(new FileStream(path, FileMode.Open, FileAccess.Read))
        {
        }

        public override string ReadLine ( )
        {
            LinesReaded++;
            return base.ReadLine();
        }

        public int LinesReaded { get; private set; }
        public int? LinesCount { get { return null; } }
        public bool EndOfLog { get { return EndOfStream; } }
    }
}