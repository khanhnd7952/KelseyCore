using UnityEngine;

namespace Kelsey
{
    public abstract class GameConfig : ScriptableObject, IGameConfig
    {
        protected const string BASE_PATH = "Config/";
        public Object GetObject() => this;
    }
}