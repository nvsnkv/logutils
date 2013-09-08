using System.Collections.Generic;

namespace NV.LogUtils.Common
{
    public interface IReport
    {
        IDictionary<IEntry, int> Entries { get; }
        int UniqueEntriesCount { get; }
        int TotalEntriesCount { get; }

    }
}