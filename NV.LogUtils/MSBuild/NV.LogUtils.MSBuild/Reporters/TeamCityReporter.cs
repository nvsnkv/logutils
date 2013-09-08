using NV.LogUtils.Common;

namespace NV.LogUtils.MSBuild.Reporters
{
    public class TeamCityReporter : IReporter
    {
        public string PrintReport(IReport report)
        {
            var output = string.Format("##teamcity[buildStatus text='{{build.status.text}}, Build warnings: {0} ({1} unique)']\r\n", report.TotalEntriesCount, report.UniqueEntriesCount);
            output += string.Format("##teamcity[buildStatisticValue key='BuildWarnings' value='{0}']\r\n", report.TotalEntriesCount);
            output += string.Format("##teamcity[buildStatisticValue key='BuildWarningsUnique' value='{0}']\r\n", report.UniqueEntriesCount);

            return output;
        }
    }
}