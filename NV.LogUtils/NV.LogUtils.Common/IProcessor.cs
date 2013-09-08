using System.Collections.Generic;

namespace NV.LogUtils.Common
{
    public interface IProcessor
    {
        IList<IEntry> GetEntries ( ILog log );
    }
}