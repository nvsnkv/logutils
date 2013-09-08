using System;
using System.IO;

namespace NV.LogUtils.Common.Logs
{
    public class LogStreamReader : StreamReader, ILog
    {
        public LogStreamReader(string path)
            :base(path)
        {
            LinesReaded = 0;
        }

        public override string ReadLine ( )
        {
            LinesReaded++;
            return base.ReadLine();
        }

        public int LinesReaded { get; private set; }
        public int? LinesCount { get { return null; } }
    }
}