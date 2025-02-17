using System;

namespace Kelsey
{
    public interface ICoinService
    {
        bool IsFirstTimeCoinChange();
        void SetCoin(int value);
        int GetCoin();
        void ConsumeCoin(int amount, string placement);
        void AddCoin(int amount, string placement);
        int GetCurrentLevel();
        void RegisterOnCoinChange(Action<int> onCoinChange);
        void UnregisterOnCoinChange(Action<int> onCoinChange);
    }
}