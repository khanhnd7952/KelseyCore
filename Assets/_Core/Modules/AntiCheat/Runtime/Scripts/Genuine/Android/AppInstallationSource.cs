﻿namespace Kelsey.AntiCheat
{
	/// <summary>
	/// Holds information about the app installation source.
	/// </summary>
	internal class AppInstallationSource
	{
		/// <summary>
		///	Package name of the installation source, for example "com.android.vending" for Google Play Store.
		/// </summary>
		internal string PackageName { get; }
		
		/// <summary>
		/// Detected source of the app installation to simplify further processing.
		/// </summary>
		internal AndroidAppSource DetectedSource { get; }
		
		internal AppInstallationSource(string packageName)
		{
			PackageName = packageName;
			DetectedSource = GetStoreName(packageName);
		}
		
		private AndroidAppSource GetStoreName(string packageName)
		{
			if (packageName == null)
				return AndroidAppSource.AccessError;
				
			switch (packageName)
			{
				case "com.android.vending":
					return AndroidAppSource.GooglePlayStore;
				case "com.amazon.venezia":
					return AndroidAppSource.AmazonAppStore;
				case "com.huawei.appmarket":
					return AndroidAppSource.HuaweiAppGallery;
				case "com.sec.android.app.samsungapps":
					return AndroidAppSource.SamsungGalaxyStore;
				case "com.google.android.packageinstaller":
				case "com.android.packageinstaller":
					return AndroidAppSource.PackageInstaller;
				default:
					return AndroidAppSource.Other;
			}
		}
	}
}