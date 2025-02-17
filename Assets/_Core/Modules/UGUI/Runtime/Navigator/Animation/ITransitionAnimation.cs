using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kelsey.UGUI
{
    public interface ITransitionAnimation
    {
        public void SetUp(RectTransform rectTransform);
        public void Prepare();
        public UniTask PlayAsync();
    }
}