using System.Collections.Generic;

namespace NV.LogUtils.Common
{
    public interface IAnalyzer
    {
        IReport Analyze ( IList<IEntry> entries );
    }
}