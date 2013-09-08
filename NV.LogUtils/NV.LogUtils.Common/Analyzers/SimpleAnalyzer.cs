using System.Collections.Generic;
using NV.LogUtils.Common.Reports;

namespace NV.LogUtils.Common.Analyzers
{
    public class SimpleAnalyzer : IAnalyzer
    {
        public IReport Analyze ( IList<IEntry> entries )
        {
            var report = new SimpleReport();

            foreach (var entry in entries)
            {
                if (report.Entries.ContainsKey(entry))
                {
                    report.Entries[entry] += 1;
                }
                else
                {
                    report.Entries.Add(entry, 1);
                }
            }

            return report;
        }
    }
}