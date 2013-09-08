namespace NV.LogUtils.Common
{
    public interface ILog
    {
        string ReadLine ( );
        int LinesReaded { get; }
        int? LinesCount { get; }

        bool EndOfLog { get; }
    }
}
