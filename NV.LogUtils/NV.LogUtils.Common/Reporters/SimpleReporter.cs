namespace NV.LogUtils.Common.Reporters
{
    public class SimpleReporter : IReporter
    {
        public string PrintReport(IReport report)
        {
            return string.Format("Entries found: {0} ({1} unique)", report.TotalEntriesCount, report.UniqueEntriesCount);
        }
    }
}