using System;
using System.Collections.Generic;

namespace Kelsey
{
    public interface ILevelPersistentDataService
    {
        int GetCurrentLevel();
        void IncreaseLevel();
        void MarkLevelGenerated(string levelId);
        void SetLevel(int level);
        bool IsLevelGenerated(string levelId);
        void RegisterOnLevelChanged(Action<int> action);
        void RegisterOnLevelGenerated(Action<string> action);
        string[] GetGeneratedLevels();
        void CheckFirstTimeInitGeneratedLevelForVersion3Dot3(List<string> levelIds);
    }
}