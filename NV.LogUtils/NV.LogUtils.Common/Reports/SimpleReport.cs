using System.Collections.Generic;
using System.Linq;

namespace NV.LogUtils.Common.Reports
{
    public class SimpleReport : IReport
    {
        public SimpleReport ( )
        {
            Entries = new Dictionary<IEntry, int>();
        }

        public IDictionary<IEntry, int> Entries { get; private set; }

        public int UniqueEntriesCount
        {
            get { return Entries.Count; }
        }
        public int TotalEntriesCount
        {
            get { return Entries.Sum(x => x.Value); }
        }
    }
}