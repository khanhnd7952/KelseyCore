using Sisus.Init;

namespace Kelsey.UGUI
{
    public class Page : View
    {
        protected override ITransitionAnimation GetDefaultEnterTransitionAnimation() => Service.Get<NavigatorDatabase>().DefaultPageEnterAnimation;
        protected override ITransitionAnimation GetDefaultExitTransitionAnimation() => Service.Get<NavigatorDatabase>().DefaultPageExitAnimation;
    }
}