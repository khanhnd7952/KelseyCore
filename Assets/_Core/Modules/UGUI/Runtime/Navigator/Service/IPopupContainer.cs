using Cysharp.Threading.Tasks;

namespace Kelsey.UGUI
{
    public interface IPopupContainer
    {
        TPopup Push<TPopup>(string key = "") where TPopup : Popup;
        UniTask<T> PushAsync<T>(string key = "") where T : Popup;
    }
}