using System.Collections.Generic;
using NV.LogUtils.Common;
using NV.LogUtils.MSBuild.Entries;

namespace NV.LogUtils.MSBuild.Processors
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