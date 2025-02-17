using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;
using UnityEngine.UI;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(RectMask2D))]
    [Service(typeof(IPageContainer), FindFromScene = true, LazyInit = true)]
    class PageContainer : MonoBehaviour, IPageContainer
    {
        [ShowInInspector, ReadOnly] private Page _currentPage;

        public async UniTask<TPage> Enter<TPage>() where TPage : Page
        {
            await Exit();

            // instantiate new page 
            _currentPage = Instantiate(View.GetView<TPage>(), transform);
            await _currentPage.Enter();

            return _currentPage as TPage;
        }

        public async UniTask Exit()
        {
            if (_currentPage != null)
            {
                await _currentPage.Exit();
                Destroy(_currentPage.gameObject);
            }
        }
    }
}