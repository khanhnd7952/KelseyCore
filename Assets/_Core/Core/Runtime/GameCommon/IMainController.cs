using System.Threading.Tasks;

namespace Kelsey
{
    public interface IMainController
    {
        Task PlayLevel(int level);
        string GetCurrentLevelId();
        void Replay();
    }
}