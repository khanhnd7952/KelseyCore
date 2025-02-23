using Cysharp.Threading.Tasks;

namespace Kelsey.UGUI
{
    public interface IPopupContainer
    {
        UniTask<TPopup> Push<TPopup>() where TPopup : PopupBase => Push<TPopup>(typeof(TPopup).Name);
        UniTask<TPopup> Push<TPopup>(string key) where TPopup : PopupBase;

        UniTask<TPopup> Push<TPopup, TData>(TData data) where TPopup : Popup<TData> =>
            Push<TPopup, TData>(typeof(TPopup).Name, data);

        UniTask<TPopup> Push<TPopup, TData>(string key, TData data) where TPopup : Popup<TData>;
    }
}