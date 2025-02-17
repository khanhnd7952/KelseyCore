using System;
using Sirenix.OdinInspector;

namespace Kelsey
{
    public interface IFirebaseSetting
    {
        RemoteConfigData[] RemoteData { get; }
    }

    [Serializable]
    public struct RemoteConfigData
    {
        [HorizontalGroup, LabelText("")] public string key;
        [HorizontalGroup, LabelText("")] public ScriptableBase value;
    }
}