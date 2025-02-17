using System;

#if KELSEY_FIREBASE_ANALYTICS
using Firebase.Analytics;
#endif

namespace Kelsey
{
    public interface IFirebaseService
    {
        Action OnFetchDone { get; set; }
        bool IsDataFetched { get; }
        bool IsFirebaseIsReady { get; }
        string GetData(string key, string defaultValue);
        T GetData<T>(string key, T defaultValue);
        string[] GetAdmobBiddingIds();
        void LogEvent(object obj);
        void LogEvent(string eventName);
#if KELSEY_FIREBASE_ANALYTICS
        void LogEvent(string eventName, Parameter[] parameters);
#endif
        void SetUserProperties(string key, string value);
    }
}