using Cysharp.Threading.Tasks;

namespace Kelsey.UGUI
{
    public interface IPageContainer
    {
        UniTask<TPage> Enter<TPage>() where TPage : Page;
        UniTask Exit();
    }
}