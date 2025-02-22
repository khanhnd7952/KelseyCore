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
        void RegisterOnCoinChange(Action<float> onCoinChange);
        void UnregisterOnCoinChange(Action<float> onCoinChange);
    }
}