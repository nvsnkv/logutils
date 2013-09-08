using System.Collections.Generic;
using NV.LogUtils.Common;

namespace NV.LogUtils.MSBuild.Entries
{
    public class MSBuildLogProcessor:IProcessor
    {
        public IList<IEntry> GetEntries(ILog log)
        {
            var result = new List<IEntry>();

            while (!log.EndOfLog)
            {
                var line = log.ReadLine();
                if (MSBuildWarning.CanCreateWarning(line))
                {
                    result.Add(MSBuildWarning.CreateWarning(line));
                }
            }

            return result;
        }
    }
}