using System;
using System.Collections.Generic;

namespace Kelsey
{
    public interface ILevelPersistentData
    {
        int GetCurrentLevel();
        void IncreaseLevel();
        void SetLevel(int level);
        void RegisterOnLevelChanged(Action<int> action);
    }
}