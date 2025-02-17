using Sisus.Init;

namespace Kelsey.UGUI
{
    public class Page : View
    {
        protected override ITransitionAnimation GetDefaultEnterTransitionAnimation() => Service<NavigatorDatabase>.Instance.DefaultPageEnterAnimation;
        protected override ITransitionAnimation GetDefaultExitTransitionAnimation() => Service<NavigatorDatabase>.Instance.DefaultPageExitAnimation;
    }
}