namespace Kelsey
{
    public interface IDebug : IGameConfig
    {
        void LogInfo(string message);
        void LogError(string message);
        void LogSilly(string message);
    }
}