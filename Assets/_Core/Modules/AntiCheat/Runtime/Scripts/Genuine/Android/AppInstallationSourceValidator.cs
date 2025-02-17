﻿using UnityEngine;

namespace Kelsey.AntiCheat
{
    /// <summary>
    /// Simple tool to quickly figure out installation source of the app.
    /// </summary>
    internal static class AppInstallationSourceValidator
    {
#if UNITY_ANDROID
        private static AndroidJavaClass routinesClass;
#endif

        /// <summary>
        /// Allows getting detailed information about installation source of the app.
        /// </summary>
        /// <returns>AppInstallationSource instance or null if run not on Android device or exception occured.</returns>
        internal static AppInstallationSource GetAppInstallationSource()
        {
#if UNITY_ANDROID
            try
            {
                var installerPackageName = Utils.AndroidRoutines.GetPackageInstallerName();
                return new AppInstallationSource(installerPackageName);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{AntiCheatStatic.LOG_PREFIX} Error while getting app installation source!");
                // ignored
            }
#else
			Debug.LogWarning($"{AntiCheatStatic.LOG_PREFIX} {nameof(AppInstallationSourceValidator)} is only available on Android device.");
#endif
            return null;
        }

        /// <summary>
        /// Checks if app was installed from the Google Play Store.
        /// </summary>
        /// <returns>True if app was installed from the Google Play, false otherwise. Can return false if installations source couldn't be identified.</returns>
        internal static bool IsInstalledFromGooglePlay()
        {
            var source = GetAppInstallationSource();
            return source?.DetectedSource == AndroidAppSource.GooglePlayStore;
        }
    }
}